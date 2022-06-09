namespace StoredProcedureApi.SPEndpoints
{
    public static class Endpoints
    {
        public const string SqlGetUsers = "exec dbo.sp_GetUsers @emailAddress,@passwordHash";

        public const string SqlCreateUsers = "exec dbo.sp_CreateUser @id out,@emailAddress,@passwordHash,@old,@oldProvider";
    }
}