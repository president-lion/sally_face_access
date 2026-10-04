// Names every IL2CPP method in GameAssembly.dll from Il2CppDumper's script.json.
// Run as a -preScript so analysis starts from known function entry points.
// Arg 0: path to script.json.
// @category SallyFace

import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import ghidra.app.script.GhidraScript;
import ghidra.program.model.address.Address;
import ghidra.program.model.symbol.SourceType;
import java.io.FileReader;

public class ApplyIl2CppSymbols extends GhidraScript {
    @Override
    protected void run() throws Exception {
        String path = getScriptArgs()[0];
        JsonObject root;
        try (FileReader r = new FileReader(path)) {
            root = JsonParser.parseReader(r).getAsJsonObject();
        }
        Address base = currentProgram.getImageBase();
        JsonArray methods = root.getAsJsonArray("ScriptMethod");
        int named = 0, failed = 0;
        for (JsonElement e : methods) {
            if (monitor.isCancelled()) break;
            JsonObject m = e.getAsJsonObject();
            Address a = base.add(m.get("Address").getAsLong());
            String name = m.get("Name").getAsString().replaceAll("[^A-Za-z0-9_.$<>:,`]", "_");
            try {
                createLabel(a, name, true, SourceType.IMPORTED);
                if (getFunctionAt(a) == null) createFunction(a, name);
                named++;
            } catch (Exception ex) {
                failed++;
            }
        }
        println("ApplyIl2CppSymbols: named " + named + ", failed " + failed);
    }
}
