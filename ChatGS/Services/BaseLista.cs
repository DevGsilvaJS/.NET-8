namespace ChatGS.Services
{
    public class BaseLista
    {

        List<object> listaSalvar = new List<object>();
        public void AddListaSalvar(object entity)
        {
            listaSalvar.Add(entity);
        }

        public List<object> GetListaSalvar()
        {
            return listaSalvar;
        }
    }
}
