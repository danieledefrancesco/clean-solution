package api;

import com.intuit.karate.KarateOptions;
import com.intuit.karate.junit4.Karate;
import org.junit.runner.RunWith;

@RunWith(Karate.class)
@KarateOptions(features = "classpath:api/Features")

public class TestRunner {
    // this will run all *.feature files that exist in sub-directories
    // see https://github.com/intuit/karate#naming-conventions
}