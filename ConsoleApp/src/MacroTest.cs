namespace ConsoleApp;

public class MacroTest {
    string GetMacro() {
        return 
            "%AMBox*\n" +
            "0 Rectangle with rounded corners, with rotation*\n" +
            "0 The origin of the aperture is its center*\n" +
            "0 $1 X-size*\n" +
            "0 $2 Y-size*\n" +
            "0 $3 Rounding radius*\n" +
            "0 $4 Rotation angle, in degrees counterclockwise*\n" +
            "0 Add two overlapping rectangle primitives as box body*\n" +
            "21,1,$1,$2-$3-$3,0,0,$4*\n" +
            "21,1,$1-$3-$3,$2,0,0,$4*\n" +
            "0 Add four circle primitives for the rounded corners*\n" +
            "$5=$1/2*\n" +
            "$6=$2/2*\n" +
            "$7=2x$3*\n" +
            "1,1,$7,$5-$3,$6-$3,$4*\n" +
            "1,1,$7,-$5+$3,$6-$3,$4*\n" +
            "1,1,$7,-$5+$3,-$6+$3,$4*\n" +
            "1,1,$7,$5-$3,-$6+$3,$4*%";
    }
}