using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;

namespace LYA1_Lexico3
{
    public class Lexico : Token, IDisposable
    {
        const int F = -1;
        const int E = -2;
        private StreamReader archivo;
        private StreamWriter log;

    int[,] TRAND =  
        {
        //  WS,L,D,.,E,+,-,La
            {0,1,2,8,1,8,8,8,8,10,11,12,13,17,16}, // 0
            {F,1,1,F,1,F,F,F,F,F,F,F,F,F,F}, // 1
            {F,F,2,3,5,F,F,F,F,F,F,F,F,F,F}, // 2
            {E,E,4,E,E,E,E,E,F,F,F,F,F,F,F}, // 3
            {F,F,4,F,5,F,F,F,F,F,F,F,F,F,F}, // 4
            {E,E,7,E,E,6,6,E,F,F,F,F,F,F,F}, // 5
            {E,E,7,E,E,E,E,E,F,F,F,F,F,F,F},// 6
            {F,F,7,F,F,F,F,F,F,F,F,F,F,F,F}, // 7
            {F,F,F,F,F,F,F,F,9,F,F,F,F,F,F},// 8
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},// 9
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F}, // 10
            {F,F,F,F,F,F,F,F,F,F,14,F,F,F,F}, // 11
            {F,F,F,F,F,F,F,F,F,F,F,14,F,F,F}, // 12
            {F,F,F,F,F,F,F,F,9,F,F,F,F,F,F}, // 13
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F}, // 14
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F}, // 15
            {F,F,F,F,F,F,F,F,9,F,F,F,F,F,F}, // 16
            {F,F,F,F,F,F,F,F,9,F,F,F,F,F,16}, // 17
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F}, // 18
         };
        public Lexico()
        {
            archivo = new StreamReader("prueba.cpp");
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public Lexico(string nombre)
        {
            archivo = new StreamReader(nombre);
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public void Dispose()
        {
            archivo.Close();
            log.Close();
        }
        private int columna(char c)
        {
            if (char.IsWhiteSpace(c))
                return 0;
            else if (char.ToLower(c) == 'e')
                return 4;
            else if (char.IsLetter(c))
                return 1;
            else if (char.IsAsciiDigit(c))
                return 2;
            else if (c=='.')
                return 3;
            else if (c=='+')
                return 5;
            else if (c=='-')
                return 6;
            else if (c=='=')
                return 8;
            else if (c==';')
                return 9;
            else if (c=='&')
                return 10;
            else if (c=='|')
                return 11;
            else if (c=='!')
                return 12;
            else if (c=='<')
                return 13;
            else if (c=='>')
                return 14;
            else
                return 7;
        }
        private void clasificar(int estado)
        {
            switch (estado)
            {
                case 1: setClasificacion(Tipos.Identificador); break;
                case 2: setClasificacion(Tipos.Numero); break;
                case 8: setClasificacion(Tipos.Asignacion); break;
                case 9: setClasificacion(Tipos.OpRelacional); break;
                case 10: setClasificacion(Tipos.FinSentencia); break;       
                case 11: setClasificacion(Tipos.Caracter); break;
                case 12: setClasificacion(Tipos.Caracter); break;
                case 13: setClasificacion(Tipos.OpLogico); break;
                case 14: setClasificacion(Tipos.OpLogico); break;
                case 16: setClasificacion(Tipos.OpRelacional); break;
                case 17: setClasificacion(Tipos.OpRelacional);break;
            }
        }
        public void nextToken()
        {
            char c;
            string buffer = "";

            int estado = 0;

            while (estado >= 0)
            {
                c = (char)archivo.Peek();

                estado = TRAND[estado,columna(c)];
                clasificar(estado);
                
                if (estado >= 0)
                {
                    if (estado > 0)
                    {
                        buffer += c;    
                    }
                    archivo.Read();
                }
            }
            if (estado == E)
            {
                throw new Error("Lexico: Se espera un digito",log);
            }
            setContenido(buffer);
            log.WriteLine(getContenido() + " = " + getClasificacion());
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
            
        }
    }
}