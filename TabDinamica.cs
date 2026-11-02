using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tarea3_2_Dinamicas
{
    /// <summary>Botón de operación: texto + acción (valor, posición) → mensaje para la bitácora.</summary>
    public class Accion
    {
        public string Texto { get; }
        public Func<string, int, string> Ejecutar { get; }

        public Accion(string texto, Func<string, int, string> ejecutar)
        {
            Texto = texto;
            Ejecutar = ejecutar;
        }
    }

    /// <summary>
    /// Pestaña reutilizable para estructuras dinámicas: genera un botón por cada Accion,
    /// dibuja los nodos y mantiene una bitácora de operaciones.
    /// </summary>
    public class TabDinamica : TabPage
    {
        private readonly Func<string[]> _valores;
        private readonly Func<string> _info;
        private readonly string _enlace, _izq, _der;

        private readonly TextBox txtValor = new TextBox { Width = 120 };
        private readonly NumericUpDown numPos = new NumericUpDown { Minimum = 0, Maximum = 999, Width = 60 };
        private readonly Label lblEstado = new Label
        {
            Dock = DockStyle.Top, Height = 34, Padding = new Padding(8, 8, 0, 0),
            Font = new Font("Consolas", 9.5f), BackColor = Color.FromArgb(245, 245, 245)
        };
        private readonly ListBox lstBitacora = new ListBox { Dock = DockStyle.Bottom, Height = 150, IntegralHeight = false };
        private readonly VisualizadorNodos visor = new VisualizadorNodos { Dock = DockStyle.Fill };

        public TabDinamica(string titulo, Accion[] acciones, Func<string[]> valores, Func<string> info,
                           string enlace, string izq, string der, bool mostrarPosicion)
        {
            Text = titulo;
            _valores = valores; _info = info; _enlace = enlace; _izq = izq; _der = der;

            FlowLayoutPanel barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 118, Padding = new Padding(6), WrapContents = true };
            barra.Controls.Add(new Label { Text = "Valor:", AutoSize = true, Margin = new Padding(6, 9, 2, 0) });
            barra.Controls.Add(txtValor);
            if (mostrarPosicion)
            {
                barra.Controls.Add(new Label { Text = "Posición:", AutoSize = true, Margin = new Padding(14, 9, 2, 0) });
                barra.Controls.Add(numPos);
            }

            foreach (Accion accion in acciones)
            {
                Accion actual = accion;   // Copia local para la lambda
                Button boton = new Button
                {
                    Text = actual.Texto, AutoSize = true,
                    Margin = new Padding(4, 3, 4, 3), Padding = new Padding(6, 2, 6, 2)
                };
                boton.Click += (s, e) => Ejecutar(actual);
                barra.Controls.Add(boton);
            }

            Controls.Add(visor);
            Controls.Add(lstBitacora);
            Controls.Add(lblEstado);
            Controls.Add(barra);

            Actualizar();
        }

        private void Ejecutar(Accion accion)
        {
            try
            {
                string mensaje = accion.Ejecutar(txtValor.Text.Trim(), (int)numPos.Value);
                Registrar(mensaje + " ✔");
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
            {
                Registrar("⚠ " + ex.Message);
            }
            Actualizar();
        }

        private void Actualizar()
        {
            visor.Mostrar(_valores(), _enlace, _izq, _der);
            lblEstado.Text = _info();
        }

        private void Registrar(string mensaje) =>
            lstBitacora.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {mensaje}");
    }
}
