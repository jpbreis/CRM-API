namespace CRMAPI.Utilities
{
    public static class CRMMessages
    {
        #region Errors
        public static string GetError(this string item, string error)
        {
            return $"Ocorreu um erro ao recuperar o item: {item}.\nEntre em concato com o suporte.\n\nERRO:\n{error}";
        }

        public static string PostError(this string item, string error)
        {
            return $"Ocorreu um erro ao criar o item: {item}.\nEntre em concato com o suporte.\n\nERRO:\n{error}";
        }

        public static string PutError(this string item, string error)
        {
            return $"Ocorreu um erro ao alterar o item: {item}.\nEntre em concato com o suporte.\n\nERRO:\n{error}";
        }

        public static string DeleteError(this string item, string error)
        {
            return $"Ocorreu um erro ao deletar o item: {item}.\nEntre em concato com o suporte.\n\nERRO:\n{error}";
        }
        #endregion

        #region Attention
        public static string PostMissingItems(this string item, string list)
        {
            return $"Para adicionar um novo {item} os dados abaixo são necessários: {list}";
        }

        public static string NotFind(this string item)
        {
            return $"{item} não encontrado.";
        }
        #endregion

        #region Ok
        public static string DeleteOk(this string item, string tabela)
        {
            return $"{item} deletado com sucesso de {tabela}";
        }
        #endregion
    }
}
