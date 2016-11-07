using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Task_1
{
    class InputClass
    {
        string file_name;
        int size;
        List<string> text_in = new List<string> { };
        string[] text_mass;

        public InputClass() { }

        public bool covert_text(string[] text_mass, int size)
        {
            this.text_mass = text_mass;
            this.size = size;
        }

        public bool convert_text(string file_name)
        {
            this.file_name = file_name;
        }


    }
}
