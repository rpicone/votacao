﻿GRANT CONNECT
ON DATABASE votacao
TO votacao;

GRANT SELECT, INSERT, UPDATE, DELETE
ON ALL TABLES IN SCHEMA public 
TO votacao;

GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO votacao;