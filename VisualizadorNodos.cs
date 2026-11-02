using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tarea3_2_Dinamicas
{
    /// <summary>
    /// Dibuja una estructura enlazada como una cadena de nodos con flechas.
    /// enlace "→" = simple, "⇄" = doble. Tiene desplazamiento horizontal automático.
    /// </summary>
    public class VisualizadorNodos : Panel
    {
        private const int NodoAncho = 76, NodoAlto = 44, Espacio = 44, Margen = 20, ZonaNull = 70;

        private string[] _valores = new string[0];
        private string _enlace = "→", _izq = "", _der = "";

        public VisualizadorNodos()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            AutoScroll = true;
            BackColor = Color.White;
            Scroll += (s, e) => Invalidate();
        }

        public void Mostrar(string[] valores, string enlace, string izq, string der)
        {
            _valores = valores ?? new string[0];
            _enlace = enlace; _izq = izq; _der = der;
            bool doble = _enlace == "⇄";
            int total = Margen * 2 + (doble ? ZonaNull : 0) + Math.Max(1, _valores.Length) * (NodoAncho + Espacio) + ZonaNull;
            AutoScrollMinSize = new Size(total, 0);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

            bool doble = _enlace == "⇄";
            int n = _valores.Length;
            int y = Height / 2 - NodoAlto / 2 + 12;
            int x0 = Margen + (doble ? ZonaNull : 0);

            using (Font fuente = new Font("Segoe UI", 10f))
            using (Font flecha = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (Font negrita = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (StringFormat centro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                if (n == 0)
                {
                    g.DrawString("(estructura vacía)  →  null", fuente, Brushes.Gray, new Rectangle(Margen, y, 320, NodoAlto), centro);
                    return;
                }

                if (doble)
                    g.DrawString("null ←", fuente, Brushes.Gray, new Rectangle(Margen, y, ZonaNull, NodoAlto), centro);

                for (int i = 0; i < n; i++)
                {
                    int x = x0 + i * (NodoAncho + Espacio);
                    Rectangle nodo = new Rectangle(x, y, NodoAncho, NodoAlto);

                    using (SolidBrush relleno = new SolidBrush(Color.FromArgb(198, 239, 206)))
                        g.FillRectangle(relleno, nodo);
                    g.DrawRectangle(Pens.Black, nodo);
                    g.DrawString(_valores[i], fuente, Brushes.Black, nodo, centro);

                    if (i < n - 1)
                        g.DrawString(_enlace, flecha, Brushes.DimGray, new Rectangle(x + NodoAncho, y, Espacio, NodoAlto), centro);
                }

                // Rótulos de extremos (cabeza/cola, tope, frente/final)
                int xUltimo = x0 + (n - 1) * (NodoAncho + Espacio);
                if (n == 1 && _izq.Length > 0 && _der.Length > 0)
                {
                    DibujarRotulo(g, negrita, centro, _izq + " / " + _der, x0, y);
                }
                else
                {
                    if (_izq.Length > 0) DibujarRotulo(g, negrita, centro, _izq, x0, y);
                    if (_der.Length > 0 && n > 1) DibujarRotulo(g, negrita, centro, _der, xUltimo, y);
                }

                g.DrawString("→ null", fuente, Brushes.Gray, new Rectangle(xUltimo + NodoAncho, y, ZonaNull, NodoAlto), centro);
            }
        }

        private static void DibujarRotulo(Graphics g, Font negrita, StringFormat centro, string texto, int x, int y)
        {
            g.DrawString(texto, negrita, Brushes.Firebrick, new Rectangle(x - 10, y - 46, NodoAncho + 20, 20), centro);
            g.DrawString("▼", negrita, Brushes.Firebrick, new Rectangle(x, y - 26, NodoAncho, 20), centro);
        }
    }
}
