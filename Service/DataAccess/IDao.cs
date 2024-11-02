public interface IDao{
    public enum SortType{
    Ascending,
    Descending
    }

    public Tuple<List<Shoes>, int> getShoes(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions);

    public Tuple<List<Order>, int> getOrders(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions);

    public Tuple<List<Category>, int> getCategories(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions);
    
    public Tuple<List<OrderDetail>, int> getOrderDetailsByID(
        int orderID,
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions);
    
    public Tuple<List<User>, int> getUsers(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions);

    public User getUserByID(
        int userID);
}