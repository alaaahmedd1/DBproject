import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class DBConnection {
    private static final String URL = "jdbc:sqlserver://localhost:1433;databaseName=SpaManagement;encrypt=false;trustServerCertificate=true";
    private static final String USER = "alaa";
    private static final String PASSWORD = "Alaa@123";

    public static Connection getConnection() throws SQLException {
        return DriverManager.getConnection(URL, USER, PASSWORD);
    }
}