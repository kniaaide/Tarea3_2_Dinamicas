using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tarea3_2_Dinamicas
{
    /// <summary>
    /// Pestaña que ejecuta un micro-benchmark real: estructura estática vs. dinámica
    /// (tiempo de Push/Pop, memoria consumida y costo de acceso por posición).
    /// Los resultados alimentan el documento técnico comparativo.
    /// </summary>
    public class TabComparativo : TabPage
    {
        private readonly NumericUpDown numN = new NumericUpDown { Minimum = 1000, Maximum = 2000000, Increment = 10000, Value = 100000, Width = 100 };
        private readonly NumericUpDown numCap = new NumericUpDown { Minimum = 1000, Maximum = 5000000, Increment = 10000, Value = 200000, Width = 100 };
        private readonly TextBox txtResultados = new TextBox
        {
            Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical,
            Font = new Font("Consolas", 10f), BackColor = Color.White
        };

        public TabComparativo()
        {
            Text = "Comparativo estático vs. dinámico";

            Button btnEjecutar = new Button { Text = "Ejecutar comparativa", AutoSize = true, Margin = new Padding(12, 3, 3, 3) };
            btnEjecutar.Click += (s, e) => Ejecutar();

            FlowLayoutPanel barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(6) };
            barra.Controls.Add(new Label { Text = "Elementos a insertar (N):", AutoSize = true, Margin = new Padding(3, 9, 3, 0) });
            barra.Controls.Add(numN);
            barra.Controls.Add(new Label { Text = "Capacidad del arreglo estático:", AutoSize = true, Margin = new Padding(14, 9, 3, 0) });
            barra.Controls.Add(numCap);
            barra.Controls.Add(btnEjecutar);

            txtResultados.Text = "Pulsa «Ejecutar comparativa».\r\n\r\n" +
                                 "Sugerencia: prueba capacidad > N (desperdicio de memoria) y capacidad < N (desbordamiento).";

            Controls.Add(txtResultados);
            Controls.Add(barra);
        }

        private void Ejecutar()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int n = (int)numN.Value, capacidad = (int)numCap.Value;
                StringBuilder sb = new StringBuilder();
                long suma = 0;
                Stopwatch cronometro = new Stopwatch();

                // ============ PILA ESTÁTICA (arreglo int[]) ============
                Limpiar();
                long m0 = GC.GetTotalMemory(true);
                PilaEstatica<int> estatica = new PilaEstatica<int>(capacidad);
                long memoriaEstatica = GC.GetTotalMemory(true) - m0;

                int insertados = Math.Min(n, capacidad);
                cronometro.Restart();
                for (int i = 0; i < insertados; i++) estatica.Push(i);
                double pushEst = cronometro.Elapsed.TotalMilliseconds;
                cronometro.Restart();
                while (!estatica.IsEmpty()) suma += estatica.Pop();
                double popEst = cronometro.Elapsed.TotalMilliseconds;
                GC.KeepAlive(estatica);
                estatica = null;

                // ============ PILA DINÁMICA (nodos) ============
                Limpiar();
                long m1 = GC.GetTotalMemory(true);
                PilaDinamica<int> dinamica = new PilaDinamica<int>();
                cronometro.Restart();
                for (int i = 0; i < n; i++) dinamica.Push(i);
                double pushDin = cronometro.Elapsed.TotalMilliseconds;
                long memoriaDinamica = GC.GetTotalMemory(true) - m1;
                cronometro.Restart();
                while (!dinamica.IsEmpty()) suma += dinamica.Pop();
                double popDin = cronometro.Elapsed.TotalMilliseconds;
                GC.KeepAlive(dinamica);
                dinamica = null;

                // ============ ACCESO POR POSICIÓN (elemento central, 100 veces) ============
                Limpiar();
                int[] arreglo = new int[n];
                ListaDoble<int> lista = new ListaDoble<int>();
                for (int i = 0; i < n; i++) { arreglo[i] = i; lista.InsertarFinal(i); }
                int medio = n / 2;

                cronometro.Restart();
                for (int r = 0; r < 100; r++) suma += arreglo[medio];
                double accesoArreglo = cronometro.Elapsed.TotalMilliseconds;
                cronometro.Restart();
                for (int r = 0; r < 100; r++) suma += lista.Obtener(medio);
                double accesoLista = cronometro.Elapsed.TotalMilliseconds;
                GC.KeepAlive(arreglo); GC.KeepAlive(lista);

                // ============ INFORME ============
                long utilEstatica = (long)insertados * sizeof(int);
                sb.AppendLine("================ RESULTADOS DE LA COMPARATIVA ================");
                sb.AppendLine($"N = {n:N0} elementos (int)   |   capacidad estática = {capacidad:N0}");
                sb.AppendLine();
                sb.AppendLine("--- PILA ESTÁTICA (arreglo) ---");
                sb.AppendLine($"Insertados: {insertados:N0}   Rechazados por desbordamiento: {n - insertados:N0}");
                sb.AppendLine($"Push: {pushEst:F3} ms   |   Pop: {popEst:F3} ms");
                sb.AppendLine($"Memoria reservada: {memoriaEstatica / 1024.0:N1} KB  (~{(double)memoriaEstatica / capacidad:F1} bytes por celda reservada)");
                sb.AppendLine($"Memoria desperdiciada (celdas sin usar): {(memoriaEstatica - utilEstatica) / 1024.0:N1} KB");
                sb.AppendLine();
                sb.AppendLine("--- PILA DINÁMICA (nodos enlazados) ---");
                sb.AppendLine($"Insertados: {n:N0}   Rechazados: 0 (sin límite fijo)");
                sb.AppendLine($"Push: {pushDin:F3} ms   |   Pop: {popDin:F3} ms");
                sb.AppendLine($"Memoria usada: {memoriaDinamica / 1024.0:N1} KB  (~{(double)memoriaDinamica / n:F1} bytes por nodo)");
                sb.AppendLine($"Sobrecarga de punteros/cabeceras: {(memoriaDinamica - (long)n * sizeof(int)) / 1024.0:N1} KB sobre los datos puros");
                sb.AppendLine();
                sb.AppendLine("--- ACCESO POR POSICIÓN (elemento central × 100) ---");
                sb.AppendLine($"Arreglo  O(1): {accesoArreglo:F4} ms");
                sb.AppendLine($"Lista doble O(n): {accesoLista:F4} ms   (≈ {(accesoArreglo > 0 ? accesoLista / accesoArreglo : 0):N0} veces más lento)");
                sb.AppendLine();
                sb.AppendLine($"(suma de control: {suma})");
                sb.AppendLine("Nota: los tiempos varían según el equipo; ejecuta 3 veces y registra el promedio en el reporte.");

                txtResultados.Text = sb.ToString().Replace("\r\n", "\n").Replace("\n", "\r\n");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static void Limpiar()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
