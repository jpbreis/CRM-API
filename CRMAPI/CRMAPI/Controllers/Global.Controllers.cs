using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRMAPI.Controllers
{
    public class GlobalControllers
    {
        public interface GlobalCreate
        {
            DateTime DataCriacao { get; set; }
            int IdCriador { get; set; }
        }

        public static async Task<T> GetLastItem<T>(
            DbContext dbContext, 
            Expression<Func<T, DateTime>> dataCriacaoEx, 
            DateTime dataCriacao,
            Expression<Func<T, int>> idCriadorEx,
            int idCriador) where T : class
        {
            var table = dbContext.Set<T>();

            var query = table.AsQueryable();

            query = query.Where(x => dataCriacaoEx.Compile()(x) == dataCriacao);
            query = query.Where(x => idCriadorEx.Compile()(x) == idCriador);

            var lastItem = await query.OrderByDescending(dataCriacaoEx).FirstOrDefaultAsync();


            //var lastItem = await query.OrderByDescending(x => dataCriacao).FirstOrDefaultAsync();

            return lastItem;
        }
    }
}
