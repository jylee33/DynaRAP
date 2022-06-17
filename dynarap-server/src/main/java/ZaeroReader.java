import org.apache.logging.log4j.util.Strings;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.InputStreamReader;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class ZaeroReader {
    public static void main(String[] args) throws Exception {
        FileInputStream fis = new FileInputStream("/Users/aloepigeon/temp/ANAYSIS_ZAERO_LD_LI212A5R2_M06_00k_abc001_AtoA_MD.dat");
        BufferedReader br = new BufferedReader(new InputStreamReader(fis));
        String line = null;
        int lineCount = 1;

        String state = "before";

        double dbl = 12.003;
        System.out.println(String.format("%.05f", dbl));

        List<List<String>> parameters = new ArrayList<>();
        List<List<List<Double>>> dataList = new ArrayList<>();
        LinkedList<Double> timeSet = new LinkedList<>();

        List<String> blockParams = null;
        List<List<Double>> blockDatas = null;

        while ((line = br.readLine()) != null) {
            line = line.trim();
            lineCount++;

            String[] splitted = line.split("\\s+");
            if (splitted == null || splitted.length < 2) {
                if (!state.equals("before")) {
                    state = "before";
                }
                continue;
            }

            if (state.equals("before")) {
                if (!splitted[0].equalsIgnoreCase("UNITS"))
                    continue;

                // append parameter array
                blockParams = new ArrayList<>();
                blockDatas = new ArrayList<>();

                for (int i = 1; i < splitted.length; i++) {
                    blockParams.add(splitted[i]);
                    blockDatas.add(new ArrayList<>());
                }

                parameters.add(blockParams);
                dataList.add(blockDatas);

                state = "extract";
                continue;
            }

            if (state.equals("extract")) {
                if (!timeSet.contains(Double.parseDouble(splitted[0])))
                    timeSet.add(Double.parseDouble(splitted[0]));

                for (int i = 1; i < splitted.length; i++) {
                    if (i < splitted.length)
                        blockDatas.get(i - 1).add(Double.parseDouble(splitted[i]));
                    else
                        blockDatas.get(i - 1).add(0.0);
                }
            }
        }

        System.out.println(timeSet.size());

        List<String> allParams = new ArrayList<>();
        for (List<String> p : parameters)
            allParams.addAll(p);

        List<List<Double>> allData = new ArrayList<>();
        for (List<List<Double>> d : dataList)
            allData.addAll(d);
    }
}
