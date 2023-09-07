using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileResearcher.Controls.DebugModels;
public class ByteViewerDebugModel
{
    public IEnumerable<int> Offsets => Enumerable.Range(0, OffsetCount).Select(i => i * ByteWidth);
    public int OffsetCount => (Bytes.Length / ByteWidth) + 1;
    public int ByteWidth => 16;
    public byte[] Bytes { get; }
    public ByteViewerDebugModel()
    {
        Bytes = File.ReadAllBytes(@"C:\Users\ChrisG\Desktop\BlurModified\models\vehicles\corvette_c3_rat.model").Take(5000).ToArray();
        //Bytes = new byte[] { 0, 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45
        //      , 0, 0, 0, 1, 6, 1, 67, 7, 32, 45, 24, 62, 4, 64, 34, 123, 46, 24, 53, 45 }; 

    }
}
