using System.Runtime.InteropServices;

namespace QuickFind.OCR
{
    public class PrScrn_Dll
    {
        [DllImport("PrScrn.dll", EntryPoint = "PrScrn")]

        public static extern int PrScrn();//与dll中一致   


    }
}
