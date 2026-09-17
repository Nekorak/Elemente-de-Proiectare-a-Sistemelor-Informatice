/* =====================================================================
   00_RunAll.sql
   Rulează toate scripturile în ordine (SQLCMD mode).

   Din linia de comandă, din folderul "SQL Code":
     sqlcmd -S <server>,<port> -U <dba> -f 65001 -i 00_RunAll.sql

   Din SSMS: Query > SQLCMD Mode, completați variabilele de mai jos, F5.

   Ordine:
     01 bază de date   02 schemă        03 tabele        04 indici
     05 funcții        06 views         07 proceduri     08 securitate
     09 joburi Agent   11 seed
   10_BackupRestore.sql se rulează separat, după seed (test de restaurare).
   ===================================================================== */

:on error exit

-- Folderul cu scripturi (cale absolută dacă rulați din SSMS)
:setvar ScriptsPath "."

-- Completați local, NU salvați parolele în Git (vezi Credentials.txt)
:setvar ParolaApp "<completeaza>"
:setvar ParolaRapoarte "<completeaza>"
:setvar BackupPath "D:\Backup\autogara"

:r $(ScriptsPath)\01_CreateDatabase.sql
:r $(ScriptsPath)\02_Schema.sql
:r $(ScriptsPath)\03_Tables.sql
:r $(ScriptsPath)\04_Indexes.sql
:r $(ScriptsPath)\05_Functions.sql
:r $(ScriptsPath)\06_Views.sql
:r $(ScriptsPath)\07_StoredProcedures.sql
:r $(ScriptsPath)\08_Security.sql
:r $(ScriptsPath)\09_Jobs.sql
:r $(ScriptsPath)\11_Seed.sql
