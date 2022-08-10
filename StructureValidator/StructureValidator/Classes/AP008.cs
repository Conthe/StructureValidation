using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureValidator
{
    public class UR008
    {
        public string CredenciadoraOuSubCred { get; set; }
        public string ConstituicaoUR { get; set; }
        public string UsuarioFinalRecebedor { get; set; }
        public string ArranjoDePagamento { get; set; }
        public DateTime DataLiquidacao { get; set; }
        public string TitularUnidadeRecebivel { get; set; }
        public string ReferenciaExternaUR { get; set; }
        public string IDContratoGeradorEvento { get; set; }
        public DateTime DataHoraEvento { get; set; }


        public List<Efeito008> listaEfeitos { get; set; } = new List<Efeito008>();

        public UR008(string credenciadoraOuSubCred, 
            string constituicaoUR, 
            string usuarioFinalRecebedor, 
            string arranjoDePagamento, 
            DateTime dataLiquidacao, 
            string titularUnidadeRecebivel, 
            string referenciaExternaUR, 
            string iDContratoGeradorEvento, 
            DateTime dataHoraEvento)
        {
            CredenciadoraOuSubCred = credenciadoraOuSubCred;
            ConstituicaoUR = constituicaoUR;
            UsuarioFinalRecebedor = usuarioFinalRecebedor;
            ArranjoDePagamento = arranjoDePagamento;
            DataLiquidacao = dataLiquidacao;
            TitularUnidadeRecebivel = titularUnidadeRecebivel;
            ReferenciaExternaUR = referenciaExternaUR;
            IDContratoGeradorEvento = iDContratoGeradorEvento;
            DataHoraEvento = dataHoraEvento;
        }
    }
}
