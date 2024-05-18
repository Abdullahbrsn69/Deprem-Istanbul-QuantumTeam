using Qu.Dto;

namespace Qu.Data
{
    public partial class dbUser
    {
        #region Get

        public static async Task<User> Get(int userid)
        {
            return await DbBase.Get<UserFull>("fn_user_get", new object[] { userid });
        }

        public static async Task<User> Get(string email)
        {
            return await DbBase.Get<UserFull>("fn_user_get_1", new object[] { email });
        }

        #endregion

        #region Insert

        public static async Task<int> Insert(User o)
        {
            return await DbBase.ExecuteNonQuery("fn_user_insert",
                          new object[] { o.name, o.surname, o.id_no, o.email, o.password, o.gsm, o.gender_type, o.bank_account_code, o.working_with_bankid, o.iban, o.logo_path, o.birthday });
        }

        #endregion

        #region Update

        public static async Task<int> Update(User o)
        {
            return await DbBase.ExecuteNonQuery("fn_user_update",
                new object[] { o.userid, o.name, o.surname, o.id_no, o.password, o.gsm, o.gender_type, o.bank_account_code, o.working_with_bankid, o.iban, o.logo_path, o.birthday, o.is_active });
        }

        #endregion
    }
}
