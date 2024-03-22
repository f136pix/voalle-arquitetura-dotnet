using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using GrupoVoalle.CrossCutting.Core.Bus;
using GrupoVoalle.CrossCutting.Core.Cqrs.Notifications;
using GrupoVoalle.Domain.Core.Interfaces;

namespace GrupoVoalle.Treinamento.Infra.DataContext
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly object _padlock = new object();

        private bool _inAnotherTransaction;
        private bool _hasError;
        private IDbContextTransaction _trans;

        private readonly CommitTransactionNotificationHandler _notifications;
        private readonly IMediatorHandler _bus;
        private readonly GrupoVoalleContext _context;

        public UnitOfWork(
            GrupoVoalleContext context,
            INotificationHandler<CommitTransactionNotification> notifications,
            IMediatorHandler bus
        )
        {
            _context = context;
            _bus = bus;
            _notifications = (CommitTransactionNotificationHandler)notifications;
        }

        public bool Commit()
        {
            return Commit(false);
        }

        public Task<bool> CommitAsync()
        {
            return CommitAsync(false);
        }

        public bool Commit(bool detachAll)
        {
            if (_trans == null)
            {
                lock (_padlock)
                {
                    _trans ??= _context.Database.BeginTransaction();
                }
            }

            try
            {
                var ret = _context.SaveChanges() > 0;

                if (detachAll)
                    _context.DetachAll();

                return ret;
            }
            catch (Exception)
            {
                _hasError = true;
                throw;
            }
        }

        public async Task<bool> CommitAsync(bool detachAll)
        {
            if (_trans == null)
            {
                lock (_padlock)
                {
                    _trans ??= _context.Database.BeginTransaction();
                }
            }

            try
            {
                var ret = (await _context.SaveChangesAsync()) > 0;

                if (detachAll)
                    _context.DetachAll();

                return ret;
            }
            catch (Exception)
            {
                _hasError = true;
                throw;
            }
        }

        public void BeginTransaction()
        {
            if (_trans != null)
            {
                _inAnotherTransaction = true;
                return;
            }

            _hasError = false;

            //_trans = _context.Database.BeginTransaction();
            lock (_padlock)
            {
                _trans ??= _context.Database.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (_inAnotherTransaction)
                return;

            if (_trans == null)
            {
                SendNotifications(_hasError ? CommitTransactionAction.Rollback : CommitTransactionAction.Commit);
            }
            else
            {
                try
                {
                    if (_hasError)
                    {
                        _trans.Rollback();
                        SendNotifications(CommitTransactionAction.Rollback);
                    }
                    else
                    {
                        _trans.Commit();
                        SendNotifications(CommitTransactionAction.Commit);
                    }
                }
                catch (Exception)
                {
                    SendNotifications(CommitTransactionAction.Error);
                    throw;
                }
            }

            ClearNotifications();

            _hasError = false;
        }

        public async Task CommitTransactionAsync()
        {
            if (_inAnotherTransaction)
                return;

            if (_trans == null)
            {
                SendNotifications(_hasError ? CommitTransactionAction.Rollback : CommitTransactionAction.Commit);
            }
            else
            {
                try
                {
                    if (_hasError)
                    {
                        await _trans.RollbackAsync();
                        SendNotifications(CommitTransactionAction.Rollback);
                    }
                    else
                    {
                        await _trans.CommitAsync();
                        SendNotifications(CommitTransactionAction.Commit);
                    }
                }
                catch (Exception)
                {
                    SendNotifications(CommitTransactionAction.Error);
                    throw;
                }
            }

            ClearNotifications();

            _hasError = false;
        }

        private void SendNotifications(CommitTransactionAction action)
        {
            if (_notifications == null)
                return;

            foreach (var notification in _notifications.GetNotifications(action))
                _bus.SendCommand(notification.Command);
        }

        private void ClearNotifications()
        {
            _notifications.Clear();
        }

        public void RollBackTransaction()
        {
            if (_trans != null)
                _trans.Rollback();

            ClearNotifications();

            _hasError = false;
        }

        public async Task RollBackTransactionAsync()
        {
            if (_trans != null)
                await _trans.RollbackAsync();

            ClearNotifications();

            _hasError = false;
        }

        public bool HasError()
        {
            return _hasError;
        }

        public void ThereIsError()
        {
            _hasError = true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}