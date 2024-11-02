public class PgSQLDao : IDao
{
    private SqlConnection dbConnection=null;


    public PgSQLDao(){
        string connectionConfig = """
            Server = localhost;
            Database = supershop;
            User Id = sa;
            Password = SqlServer@123;
            TrustServerCertificate = True;
        """;
        var dbConnection= new NpgsqlConnection(connectionConfig);
        dbConnection.Open();
    }

    public PgSQLDao(string serverUrl, string databaseName, string userId, string password){
        string connectionConfig = $"""
            Server = {serverUrl};
            Database = {databaseName};
            User Id = {userId};
            Password = {password};
            TrustServerCertificate = True;
        """;
        var dbConnection= new NpgsqlConnection(connectionConfig);
        dbConnection.Open();
    }

    public ~SQLDao(){
        dbConnection.Close();
    }


    private string getSortString(string[] sortFields,Dictionary<string, SortType> sortOptions){
        string sortString = "ORDER BY ";
        bool useDefaultSort = true;
        int countSortFields = 0;
        foreach(var item in sortOptions) {
            if (sortFields.Contains(item.Key)) {
                useDefault = false;
                if(countSortFields > 0){
                    sortString += ", ";
                }
                if (item.Value == SortType.Ascending) {
                    sortString += $"{item.Key} asc";
                } else {
                    sortString += $"{item.Key} desc";
                }
                countSortFields++;
            }
        }
        if (useDefaultSort) {
            sortString += "ID";
        }
        return sortString;
    }

    private string getWhereString(string[] whereFields,Dictionary<string, string> whereOptions){
        string whereString = "WHERE ";
        bool useDefaultWhere = true;
        int countWhereFields = 0;
        foreach(var item in whereOptions) {
            if (whereFields.Contains(item.Key)) {
                useDefaultWhere = false;
                if(countWhereFields > 0){
                    whereString += " AND ";
                }
                whereString += $"{item.Key} LIKE {item.Value}";
                countWhereFields++;
            }
        }
        if (useDefaultWhere) {
            whereString = "";
        }
        return whereString;
    }

    public Tuple<List<Shoes>, int> getShoes(
        int page, int rowsPerPage,
        Dictionary<string,string> whereOptions,
        Dictionary<string, SortType> sortOptions){
        
        var string[] shoesFields = new string[]{
            "ID", "CategoryID", "Name", "Size", "Color", "Price", "Stock"
        };
        var result = new List<Shoes>();
        string sortString = getSortString(shoesFields,sortOptions);
        string whereString = getWhereString(shoesFields,whereOptions);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, ID, CategoryID, 
            Name, Size, Color, Price, Stock, Avatar
            FROM Shoes
            {whereString}
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
        var command = new NpgsqlCommand(sqlQuery, connection);
        command.Parameters.Add("@Skip", SqlDbType.Int)
            .Value = (page - 1) * rowsPerPage;
        command.Parameters.Add("@Take", SqlDbType.Int)
            .Value = rowsPerPage;
        command.Parameters.Add("@Keyword", SqlDbType.NText)
            .Value = $"%{keyword}%";
        var reader = command.ExecuteReader();
        int totalShoes = -1;

        while (reader.Read())
        {
            if (totalShoes == -1) {
                totalShoes  = (int)reader["Total"];
             }

            var shoes = new Shoes(); 
            shoes.ID = (int)reader["ID"];
            shoes.CategoryID = (string)reader["CategoryID"];
            shoes.Name = (string) reader["Name"];
            shoes.Size = (string) reader["Size"];
            shoes.Color = (string) reader["Color"];
            shoes.Price = (decimal) reader["Price"];
            shoes.Stock = (int) reader["Stock"];
            shoes.Avatar = (string) reader["Avatar"];
            result.Add(employee);
        }
        return new Tuple<List<Employee>, int>(
            result, totalShoes
        );
    }

    
    public Tuple<List<Order>, int> getOrders(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions){
        
        var string[] orderFields = new string[]{
            "ID", "UserID", "AddressID", "OrderDate", "Status", "TotalPrice"
        };
        var result = new List<Order>();
        string sortString = getSortString(orderFields,sortOptions);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, ID, UserID, 
            AddressID, OrderDate, Status, TotalPrice
            FROM Order
            WHERE Status like @Keyword
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
        var command = new NpgsqlCommand(sqlQuery, connection);
        command.Parameters.Add("@Skip", SqlDbType.Int)
            .Value = (page - 1) * rowsPerPage;
        command.Parameters.Add("@Take", SqlDbType.Int)
            .Value = rowsPerPage;
        command.Parameters.Add("@Keyword", SqlDbType.NText)
            .Value = $"%{keyword}%";
        var reader = command.ExecuteReader();
        int totalOrders = -1;

        while (reader.Read())
        {
            if (totalOrders == -1) {
                totalOrders  = (int)reader["Total"];
             }

            var order= new Order(); // Relation -> Objects
            order.ID = (int)reader["ID"];
            order.UserID = (int)reader["UserID"];
            order.AddressID = (int)reader["AddressID"];
            order.OrderDate = (string) reader["OrderDate"];
            order.Status = (string) reader["Status"];
            order.TotalPrice = (decimal) reader["TotalPrice"];
            result.Add(order);
        }
        return new Tuple<List<Order>, int>(
            result, totalOrders
        );
    }

    public Tuple<List<Category>, int> getCategories(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions){

    }
    
    public Tuple<List<OrderDetail>, int> getOrderDetailsByID(
        int orderID,
        int page, int rowsPerPage,
        Dictionary<string,string> whereOptions,
        Dictionary<string, SortType> sortOptions){

        
        var string[] orderDetailFields = new string[]{
            "ID", "OrderID", "ShoesID", "Quantity", "Price"
        };
        var result = new List<OrderDetail>();
        const whereString = getWhereString(orderDetailFields,whereOptions);
        string sortString = getSortString(orderDetailFields,sortOptions);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, ID, OrderID, 
            ShoesID, Quantity, Price
            FROM OrderDetail
            {whereString}
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
        var command = new NpgsqlCommand(sqlQuery, connection);
        command.Parameters.Add("@Skip", SqlDbType.Int)
            .Value = (page - 1) * rowsPerPage;
        command.Parameters.Add("@Take", SqlDbType.Int)
            .Value = rowsPerPage;
        command.Parameters.Add("@OrderID", SqlDbType.Int)
            .Value = orderID;
        command.Parameters.Add("@Keyword", SqlDbType.NText)
            .Value = $"%{keyword}%";
        var reader = command.ExecuteReader();
        int totalOrderDetails = -1;

        while (reader.Read())
        {
            if (totalOrderDetails == -1) {
                totalOrderDetails  = (int)reader["Total"];
             }

            var orderDetail= new OrderDetail(); 
            orderDetail.ID = (int)reader["ID"];
            orderDetail.OrderID = (int)reader["OrderID"];
            orderDetail.ShoesID = (int)reader["ShoesID"];
            orderDetail.Quantity = (int)reader["Quantity"];
            orderDetail.Price = (decimal)reader["Price"];
            result.Add(orderDetail);
        }
        return new Tuple<List<Order>, int>(
            result, totalOrderDetails
        );
    }
    
    public Tuple<List<User>, int> getUsers(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<stirng,string> whereOptions,
        Dictionary<string, SortType> sortOptions){

        var string[] userFields = new string[]{
            "ID","AddressID" ,"Name", "Email" , "Password", "Role","PhoneNumber"
        }
        var result = new List<User>();
        string sortString = getSortString(userFields,sortOptions);
        string whereString = getWhereString(userFields,whereOptions);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, ID, Username, 
            Password, Role
            FROM User
            {whereString}
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
        var command = new NpgsqlCommand(sqlQuery, connection);
        command.Parameters.Add("@Skip", SqlDbType.Int)
            .Value = (page - 1) * rowsPerPage;
        command.Parameters.Add("@Take", SqlDbType.Int)
            .Value = rowsPerPage;
        var reader = command.ExecuteReader();
        int totalUsers = -1;

        while (reader.Read())
        {
            if (totalUsers == -1) {
                totalUsers  = (int)reader["Total"];
            }

            var User=new User(); 
            user.ID = (int)reader["ID"];
            user.AddressID = (int)reader["AddressID"];
            user.Name = (string)reader["Name"];
            user.Email = (string)reader["Email"];
            user.Password = (string)reader["Password"];
            user.Role = (string)reader["Role"];
            user.PhoneNumber = (string)reader["PhoneNumber"];
            result.Add(user);
        }
        return new Tuple<List<Order>, int>(
            result, totalUsers
        );

    }

    public User getUserByID(
        int userID){
        var sqlQuery = $"""
            SELECT ID, Username, Password, Role
            FROM User
            WHERE ID = @UserID""";
        var command = new NpgsqlCommand(sqlQuery, connection);
        command.Parameters.Add("@UserID", SqlDbType.Int)
            .Value = userID;
        var reader = command.ExecuteReader();
        User user = new User();
        user.ID = (int)reader["ID"];
        user.Username = (string)reader["Username"];
        user.Password = (string)reader["Password"];
        user.Role = (string)reader["Role"];
        return user;
    }

    
}
