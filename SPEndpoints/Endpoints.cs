namespace StoredProcedureApi.SPEndpoints
{
    public static class Endpoints
    {
       public const string SqlGetUsers = "exec dbo.sp_GetUserbyId @id"; // @emailAddress,@passwordHash";

        public const string SpGetUser = "sp_GetUserbyId";

        public const string SqlCreateUsers = "exec dbo.sp_CreateUser @emailAddress,@passwordHash,@old,@oldProvider, @useridout out";

        public const string SpCreateUsers = "sp_CreateUser";
        public const string SpUpload = "sp_Upload";
    }
}