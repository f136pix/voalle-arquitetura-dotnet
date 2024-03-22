CREATE OR REPLACE FUNCTION vll_get_friendly_name(
    table_name VARCHAR,
    column_name VARCHAR,
    column_id VARCHAR,
    id_value VARCHAR
)
RETURNS TABLE (
    name VARCHAR
)
LANGUAGE plpgsql
AS $BODY$
DECLARE
result_value VARCHAR;
    query_string VARCHAR;
BEGIN
    query_string := 'SELECT ' || quote_ident(column_name) || ' FROM ' || quote_ident(table_name) || ' WHERE ' || quote_ident(column_id) || ' = $1 LIMIT 1';

EXECUTE query_string INTO result_value USING id_value;

RETURN QUERY SELECT result_value AS name;
END;
$BODY$;

