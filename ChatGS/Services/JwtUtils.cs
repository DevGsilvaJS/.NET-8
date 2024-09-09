using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ChatGS.Auth
{
    public static class JwtUtils
    {
        public static SymmetricSecurityKey GenerateSymmetricSecurityKey(string key)
        {
            // Converte a string da chave para bytes usando UTF-8 encoding
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Verifica se a chave tem pelo menos 128 bits (16 bytes)
            if (keyBytes.Length < 128 / 8)
            {
                throw new ApplicationException("A chave JWT deve ter pelo menos 128 bits de tamanho.");
            }

            // Retorna a chave como SymmetricSecurityKey
            return new SymmetricSecurityKey(keyBytes);
        }
    }
}
