using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_STM.Properties;

namespace E_STM
{
    
    public class EdgeSelect
    {
        private bool PreviewValBool;
        private int PreviewValInt;

        public EdgeSelect (int PreviewValInt)
        {
            this.PreviewValInt = PreviewValInt;
        }
        public EdgeSelect(bool PreviewValBool)
        {
            this.PreviewValBool = PreviewValBool;
        }

        public bool pulseWhenChengedValue(int val)
        {
            if (PreviewValInt != val)
            {
                PreviewValInt = val;
                return true;
            }else
            {
                return false;
            }
        }
        public bool pulseWhenChengedValue(bool val)
        {
            if (PreviewValBool != val)
            {
                PreviewValBool = val;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
