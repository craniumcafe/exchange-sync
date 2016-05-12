using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CraniumCafeSync.Business_Logic
{
    class EnDecrypt
    {
        public string Encrypt(string encrypt)
        {
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(encrypt);
                byte[] protectedPassword = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(protectedPassword);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in EnDecrypt " + ex.Message);
                return null;
            }
        }

        public string Decrypt(string decrypt)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(decrypt);
                byte[] password = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
                return Encoding.Unicode.GetString(password);
            }
            catch (Exception ex)
            {
                Debug.DebugMessage(2, "Error in EnDecrypt " + ex.Message);
                return null;
            }

        }
    }
}
