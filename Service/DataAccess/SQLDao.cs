public class SQLDao : IDao
{
    private SqlConnection dbConnection=null;

    public SQLDao(){
        string connectionConfig = """
            Server = localhost;
            Database = supershop;
            User Id = sa;
            Password = SqlServer@123;
            TrustServerCertificate = True;
        """;
        var dbConnection= new SqlConnection(connectionConfig);
        dbConnection.Open();
    }

    public ~SQLDao(){
        dbConnection.Close();
    }


    private string getSortStringOfOrder(string[] sortFields,Dictionary<string, SortType> sortOptions){
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

    public Tuple<List<Shoes>, int> getShoes(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions){
        
        var string[] shoesFields = new string[]{
            "ID", "CategoryID", "Name", "Size", "Color", "Price", "Stock"
        };
        var result = new List<Shoes>();
        string sortString = getSortStringOfShoes(shoesFields,sortOptions);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, ID, CategoryID, 
            Name, Size, Color, Price, Stock, Avatar
            FROM Shoes
            WHERE Name like @Keyword
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
        var command = new SqlCommand(sqlQuery, connection);
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

            var shoes = new Shoes(); // Relation -> Objects
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
        var command = new SqlCommand(sqlQuery, connection);
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
            result.Add(employee);
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
        string keyword,
        Dictionary<string, SortType> sortOptions){

        
        var string[] orderDetailFields = new string[]{
            "ID", "OrderID", "ShoesID", "Quantity", "Price"
        };
        var result = new List<OrderDetail>();
        string sortString = getSortString(orderDetailFields,sortOptions);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, ID, OrderID, 
            ShoesID, Quantity, Price
            FROM OrderDetail
            WHERE OrderID = @OrderID
            WHERE Quantity like @Keyword
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;
        var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.Add("@Skip", SqlDbType.Int)
            .Value = (page - 1) * rowsPerPage;
        command.Parameters.Add("@Take", SqlDbType.Int)
            .Value = rowsPerPage;
        command.Parameters.Add("@OrderID", SqlDbType.Int)
            .Value = orderID;
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
            result.Add(employee);
        }
        return new Tuple<List<Order>, int>(
            result, totalOrders
        );
    }
    
    public Tuple<List<User>, int> getUsers(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions);

    public User getUserByID(
        int userID);

    
}
