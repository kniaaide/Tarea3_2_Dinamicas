using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tarea3_2_Dinamicas
{
    /// <summary>
    /// Formulario principal de la Tarea 3.2: lista doble, pila dinámica, cola dinámica
    /// y pestaña comparativa estático vs. dinámico.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly ListaDoble<string> _lista = new ListaDoble<string>();
        private readonly PilaDinamica<string> _pila = new PilaDinamica<string>();
        private readonly ColaDinamica<string> _cola = new ColaDinamica<string>();

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>Valida que el cuadro "Valor" no esté vacío.</summary>
        private static string Req(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Escribe un valor en el cuadro «Valor».");
            return valor;
        }
    }
}
