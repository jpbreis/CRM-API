using CRMAPI.Controllers;
using CRMAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<OperadorModel> Operadores => Set<OperadorModel>();
        public DbSet<TK_NewOperador> tK_NewOperador => Set<TK_NewOperador>();
        public DbSet<LeadModel> bdLead => Set<LeadModel>();
        public DbSet<DepartamentoModel> bdDepartamento => Set<DepartamentoModel>();
        public DbSet<AcompanhamentoLeadModel> bdAcompanhamentoLead => Set<AcompanhamentoLeadModel>();
        public DbSet<NivelAcessoModel> bdNivelAcesso => Set<NivelAcessoModel>();
        public DbSet<DependenteModel> bdDependente => Set<DependenteModel>();
        public DbSet<CargoModel> bdCargo => Set<CargoModel>();
        public DbSet<ContratoTrabalhoModel> bdContratoTrabalho => Set<ContratoTrabalhoModel>();
        public DbSet<JornadaTrabalhoModel> bdJornadaTrabalho => Set<JornadaTrabalhoModel>();
        public DbSet<MidiaModel> bdMidia => Set<MidiaModel>();
        public DbSet<ClienteModel> bdCliente => Set<ClienteModel>();
        public DbSet<OperadoresComplModel> OperadoresCompl => Set<OperadoresComplModel>();
        public DbSet<EmpresaModel> bdEmpresa => Set<EmpresaModel>();
        public DbSet<GrupoEmpresaModel> bdGrupoEmpresa => Set<GrupoEmpresaModel>();
        public DbSet<AssociacaoGrupoEmpresaModel> bdAssociacaoGrupoEmpresa => Set<AssociacaoGrupoEmpresaModel>();
        public DbSet<AdicionaisProdutoModel> bdAdicionaisProduto => Set<AdicionaisProdutoModel>();
        public DbSet<FormaPagamentoModel> bdFormaPagamento => Set<FormaPagamentoModel>();
        public DbSet<ProdutoModel> bdProduto => Set<ProdutoModel>();
        public DbSet<VendaModel> bdVenda => Set<VendaModel>();
        public DbSet<BancosModel> bdBancos => Set<BancosModel>();
    }
}
