using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tarea3_2_Dinamicas
{
    partial class MainForm
    {
        /// <summary>
        /// Construye la interfaz (equivalente al código generado por el diseñador).
        /// Visual Studio usa este archivo al abrir MainForm.cs en "Ver diseñador".
        /// </summary>
        private void InitializeComponent()
        {
            Text = "Unidad 3 · Tarea 3.2 — Estructuras Dinámicas (Lista, Pila, Cola) y Comparativa";
            ClientSize = new Size(1040, 660);
            MinimumSize = new Size(900, 580);
            StartPosition = FormStartPosition.CenterScreen;

            TabControl tabs = new TabControl { Dock = DockStyle.Fill };

            // ---------------- LISTA DOBLEMENTE ENLAZADA ----------------
            tabs.TabPages.Add(new TabDinamica(
                "Lista doblemente enlazada",
                new[]
                {
                    new Accion("Insertar inicio",    (v, p) => { _lista.InsertarInicio(Req(v)); return $"InsertarInicio(\"{v}\")  [O(1)]"; }),
                    new Accion("Insertar final",     (v, p) => { _lista.InsertarFinal(Req(v));  return $"InsertarFinal(\"{v}\")  [O(1)]"; }),
                    new Accion("Insertar en posición", (v, p) => { _lista.InsertarEn(p, Req(v)); return $"InsertarEn({p}, \"{v}\")  [O(n)]"; }),
                    new Accion("Eliminar inicio",    (v, p) => $"EliminarInicio() → \"{_lista.EliminarInicio()}\"  [O(1)]"),
                    new Accion("Eliminar final",     (v, p) => $"EliminarFinal() → \"{_lista.EliminarFinal()}\"  [O(1)]"),
                    new Accion("Eliminar en posición", (v, p) => $"EliminarEn({p}) → \"{_lista.EliminarEn(p)}\"  [O(n)]"),
                    new Accion("Eliminar por valor", (v, p) => _lista.EliminarValor(Req(v)) ? $"EliminarValor(\"{v}\")" : $"EliminarValor(\"{v}\") → no encontrado"),
                    new Accion("Buscar valor",       (v, p) => { int i = _lista.Buscar(Req(v)); return i >= 0 ? $"Buscar(\"{v}\") → índice {i}" : $"Buscar(\"{v}\") → no encontrado"; }),
                    new Accion("Obtener posición",   (v, p) => $"Obtener({p}) → \"{_lista.Obtener(p)}\""),
                    new Accion("Invertir",           (v, p) => { _lista.Invertir(); return "Invertir()  [O(n)]"; }),
                    new Accion("Limpiar",            (v, p) => { _lista.Limpiar(); return "Limpiar()"; })
                },
                () => _lista.ToArray(),
                () => $"Count={_lista.Count}   (cabeza ⇄ … ⇄ cola)",
                "⇄", "cabeza", "cola", true));

            // ---------------- PILA DINÁMICA ----------------
            tabs.TabPages.Add(new TabDinamica(
                "Pila dinámica (LIFO)",
                new[]
                {
                    new Accion("Push(valor)", (v, p) => { _pila.Push(Req(v)); return $"Push(\"{v}\")"; }),
                    new Accion("Pop()",       (v, p) => $"Pop() → \"{_pila.Pop()}\""),
                    new Accion("Peek()",      (v, p) => $"Peek() → \"{_pila.Peek()}\""),
                    new Accion("IsEmpty()",   (v, p) => $"IsEmpty() → {_pila.IsEmpty()}"),
                    new Accion("Limpiar",     (v, p) => { _pila.Clear(); return "Clear()"; })
                },
                () => _pila.ToArray(),
                () => $"Count={_pila.Count}   (sin límite fijo: no existe IsFull)",
                "→", "TOPE", "BASE", false));

            // ---------------- COLA DINÁMICA ----------------
            tabs.TabPages.Add(new TabDinamica(
                "Cola dinámica (FIFO)",
                new[]
                {
                    new Accion("Enqueue(valor)", (v, p) => { _cola.Enqueue(Req(v)); return $"Enqueue(\"{v}\")"; }),
                    new Accion("Dequeue()",      (v, p) => $"Dequeue() → \"{_cola.Dequeue()}\""),
                    new Accion("Peek()",         (v, p) => $"Peek() → \"{_cola.Peek()}\""),
                    new Accion("IsEmpty()",      (v, p) => $"IsEmpty() → {_cola.IsEmpty()}"),
                    new Accion("Limpiar",        (v, p) => { _cola.Clear(); return "Clear()"; })
                },
                () => _cola.ToArray(),
                () => $"Count={_cola.Count}   (frente → … → final)",
                "→", "FRENTE", "FINAL", false));

            // ---------------- COMPARATIVA ----------------
            tabs.TabPages.Add(new TabComparativo());

            Controls.Add(tabs);
        
        }
    }
}
