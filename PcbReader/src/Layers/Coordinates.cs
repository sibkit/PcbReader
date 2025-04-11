using PcbReader.Project;

namespace PcbReader.Layers;

public static class Coordinates {

    public static Coordinate ParseCoordinate(NumberFormat nf, string xString, string yString) {
        return new Coordinate(ReadValue(nf, xString), ReadValue(nf, yString));
    }
    public static decimal ReadValue(NumberFormat nf, string value) {
        if (value == "0")
            return 0;
        
        if(value[0]=='+')
            value = value[1..];
        
        var multiplier = value[0] == '-' ? -1 : 1;
        if (multiplier == -1)
            value = value[1..];
        
        string wholeNumberString, fracString;
        switch (nf.Zeros) {
            case Zeros.Leading:
                if (value.Length < nf.Left) {
                    wholeNumberString = value;
                    var delta = nf.Left!.Value-value.Length;
                    for (var i = 0; i < delta; i++) {
                        wholeNumberString += "0";
                    }
                    fracString = "";
                } else {
                    wholeNumberString = value[0..nf.Left!.Value];
                    fracString = value[nf.Left!.Value..];
                }
                break;
            case Zeros.Trailing:
                fracString = value[^nf.Right!.Value..];
                wholeNumberString = value[..^nf.Right!.Value];
                break;
            case Zeros.All:
                wholeNumberString = value[..nf.Left!.Value];
                fracString = value[^nf.Right!.Value..];
                break;
            default: throw new Exception("Coordinates: ReadValue (Unknown Zeros value: \"" + nf.Zeros + "\")");
        }

        if (wholeNumberString == "") {
            return multiplier * decimal.Parse(fracString) / (decimal)Math.Pow(10, fracString.Length);
        }

        if (fracString == "") {
            return multiplier * decimal.Parse(wholeNumberString);
        }
        return multiplier * decimal.Parse(wholeNumberString)+decimal.Parse(fracString) / (decimal)Math.Pow(10, fracString.Length);

    }
}