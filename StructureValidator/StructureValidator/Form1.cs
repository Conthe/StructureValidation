using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureValidator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] efeitos = new string[99999];
            char delimiterChar = '\n';
            StreamReader sr1;
            OpenFileDialog dlg = new OpenFileDialog();
            string fileName1 = String.Empty;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName1 = dlg.FileName;
                sr1 = new StreamReader(fileName1);

                while (!sr1.EndOfStream)
                {
                    string linha = sr1.ReadLine();
                    configuraLinhaEfeitos(ref efeitos, delimiterChar, linha);
                }
                MessageBox.Show("Validação efetuada com sucesso.");
            }
        }
        private static void configuraLinhaEfeitos(ref string[] efeitos, char delimiterChar, string dados)
        {
            string[] linhas = dados.Split(delimiterChar);
            string infoLinha = string.Empty;
            try
            {
                foreach (string linha in linhas)
                {
                    if (!string.IsNullOrEmpty(linha))
                    {
                        string texto = PegarConteudoForaAspas(linha, 0);
                        var urInfo = texto.Split(';');
                        string dataHora = urInfo[9].Substring(0, 19);
                        UR008 unidadeRecebivel = new UR008(urInfo[0], urInfo[1], urInfo[2], urInfo[3], DateTime.Parse(urInfo[4]).Date, urInfo[5], urInfo[7], urInfo[8], DateTime.Parse(dataHora));
                        string efeitoStr = PegarConteudoEntreAspas(linha, 0);
                        if (!string.IsNullOrEmpty(efeitoStr))
                        {
                            efeitos = efeitoStr.Split('|');
                            foreach (string efeito in efeitos)
                            {
                                Efeito008 efeitoLinha = new Efeito008();
                                string[] efeitoConfigurado = efeito.Split(';');
                                PopulaCamposEfeito(efeitoLinha, efeitoConfigurado);
                                unidadeRecebivel.listaEfeitos.Add(efeitoLinha);
                            }
                        }
                        ValidaEstrutura(unidadeRecebivel);
                    }
                }
            }catch(Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        static void ValidaEstrutura(UR008 unidadeRecebivel)
        {
            if (unidadeRecebivel.CredenciadoraOuSubCred.Length != 14)
                throw new Exception("Credenciadora ou SubCredenciadora invalida " 
                    + unidadeRecebivel.CredenciadoraOuSubCred + ";"
                    + unidadeRecebivel.ConstituicaoUR + ";"
                    + unidadeRecebivel.UsuarioFinalRecebedor + ";"
                    + unidadeRecebivel.ArranjoDePagamento+";");
            if (unidadeRecebivel.ConstituicaoUR.Length != 1)
                throw new Exception("ContituicaoUR invalida "
                    + unidadeRecebivel.CredenciadoraOuSubCred + ";"
                    + unidadeRecebivel.ConstituicaoUR + ";"
                    + unidadeRecebivel.UsuarioFinalRecebedor + ";"
                    + unidadeRecebivel.ArranjoDePagamento + ";");
            if (unidadeRecebivel.UsuarioFinalRecebedor.Length != 11 && unidadeRecebivel.UsuarioFinalRecebedor.Length != 14)
                throw new Exception("UsuarioFinalRecebedor invalido "
                    + unidadeRecebivel.CredenciadoraOuSubCred + ";"
                    + unidadeRecebivel.ConstituicaoUR + ";"
                    + unidadeRecebivel.UsuarioFinalRecebedor + ";"
                    + unidadeRecebivel.ArranjoDePagamento + ";");
            if (unidadeRecebivel.ArranjoDePagamento.Length != 3)
                throw new Exception("ArranjoDePagamento invalido "
                   + unidadeRecebivel.CredenciadoraOuSubCred + ";"
                    + unidadeRecebivel.ConstituicaoUR + ";"
                    + unidadeRecebivel.UsuarioFinalRecebedor + ";"
                    + unidadeRecebivel.ArranjoDePagamento + ";");
            if (unidadeRecebivel.TitularUnidadeRecebivel.Length != 11 && unidadeRecebivel.TitularUnidadeRecebivel.Length != 14)
                throw new Exception("TitularUnidadeRecebivel Invalido "
                    + unidadeRecebivel.CredenciadoraOuSubCred + ";"
                    + unidadeRecebivel.ConstituicaoUR + ";"
                    + unidadeRecebivel.UsuarioFinalRecebedor + ";"
                    + unidadeRecebivel.ArranjoDePagamento + ";");
            ValidaEstruturaEfeitos(unidadeRecebivel.listaEfeitos);
        }
        
        static void ValidaEstruturaEfeitos(List<Efeito008> listaEfeitos)
        {
            foreach(Efeito008 efeito in listaEfeitos)
            {
                string valorComVirgula = efeito.ValorComprometido.Replace('.', ',');
                double valorConvertido = 0;

                if (efeito.Protocolo.Length != 36)
                    throw new Exception("Protocolo invalido " +
                        efeito.Protocolo);
                if (efeito.IndicadorEfeitosContrato.Length != 1)
                    throw new Exception("IndicadorEfeitosContrato invalido " +
                        efeito.Protocolo);
                if (efeito.EntidadeRegistradora.Length != 14)
                    throw new Exception("EntidadeRegistradora invalida " +
                        efeito.Protocolo);
                if (efeito.TipoEfeito.Length != 1)
                    throw new Exception("TipoEfeito invalido " +
                        efeito.Protocolo);
                if (efeito.BeneficiarioTitular.Length != 11 && efeito.BeneficiarioTitular.Length != 14)
                    throw new Exception("BeneficiarioTitular invalido " +
                        efeito.Protocolo);
                if (efeito.RegrasDivisao.Length != 1)
                    throw new Exception("RegraDivisao invalida " +
                        efeito.Protocolo);
                if (!double.TryParse(valorComVirgula, out valorConvertido))
                    throw new Exception("ValorComprometido invalido " +
                        efeito.Protocolo);
                if (efeito.DocumentoTitularDomicilio.Length != 11 && efeito.DocumentoTitularDomicilio.Length != 14)
                    throw new Exception("DocumentoTitularDomicilio invalido " +
                        efeito.Protocolo);
                if (efeito.TipoConta.Length != 2)
                    throw new Exception("TipoConta invalido " +
                        efeito.Protocolo);
                if (efeito.ISPB.Length != 8)
                    throw new Exception("ISPB invalido " +
                        efeito.Protocolo);
                
            }
        }
        static string PegarConteudoForaAspas(string texto, int linha)
        {
            var existeAspas = texto.Any(x => x == '"');
            if (existeAspas)
            {
                int i, j;
                string strLinha;
                EncontraIndice(texto, linha, out i, out j, out strLinha);

                return strLinha.Remove(i, j - i);
            }
            return texto;
        }

        static string PegarConteudoEntreAspas(string texto, int linha)
        {
            var existeAspas = texto.Any(x => x == '"');
            if (existeAspas)
            {
                int i, j;
                string strLinha;
                EncontraIndice(texto, linha, out i, out j, out strLinha);

                return strLinha.Substring(i, j - i);
            }
            return string.Empty;
        }

        private static void EncontraIndice(string texto, int linha, out int i, out int j, out string strLinha)
        {
            i = 0;
            for (int x = 0; x < linha; x++) i = texto.IndexOf("\n", i) + 1; // Encontra a linha...
            j = texto.IndexOf("\n", i);
            if (j < 0) j = texto.Length - 1;

            strLinha = texto.Substring(i, j - i);
            i = strLinha.IndexOf("\"") + 1;
            j = strLinha.IndexOf("\"", i);
        }

        private static void PopulaCamposEfeito(Efeito008 efeitoLinha, string[] efeitoConfigurado)
        {
            efeitoLinha.Protocolo = efeitoConfigurado[0];
            efeitoLinha.IndicadorEfeitosContrato = efeitoConfigurado[1];
            efeitoLinha.EntidadeRegistradora = efeitoConfigurado[2];
            efeitoLinha.TipoEfeito = efeitoConfigurado[3];
            efeitoLinha.BeneficiarioTitular = efeitoConfigurado[4];
            efeitoLinha.RegrasDivisao = efeitoConfigurado[5];
            efeitoLinha.ValorComprometido = efeitoConfigurado[6];
            efeitoLinha.DocumentoTitularDomicilio = efeitoConfigurado[7];
            efeitoLinha.TipoConta = efeitoConfigurado[8];
            efeitoLinha.COMPE = efeitoConfigurado[9];
            efeitoLinha.ISPB = efeitoConfigurado[10];
            efeitoLinha.Agencia = efeitoConfigurado[11];
            efeitoLinha.NumeroContaPagamento = efeitoConfigurado[12];
            efeitoLinha.NomeTitularDomicilio = efeitoConfigurado[13];
            efeitoLinha.IDContrato = efeitoConfigurado[14];
        }
    }
}
