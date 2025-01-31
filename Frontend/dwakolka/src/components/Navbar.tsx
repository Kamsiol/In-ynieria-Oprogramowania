import { Link, useNavigate } from "react-router-dom";
import { isAuthenticated, logout } from "../components/authService";

const Navbar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <nav className="p-4 bg-gray-800 text-white">
      <ul className="flex gap-4">
        <li><Link to="/">Home</Link></li>
        {isAuthenticated() ? (
          <>
            <li><Link to="/about">About</Link></li>
            <li><Link to="/contact">Contact</Link></li>
            <li><button onClick={handleLogout} className="text-red-500">Logout</button></li>
          </>
        ) : (
          <>
          <li><Link to="/login">Login</Link></li>
          <li><Link to="/register">Register</Link></li>
          <li><Link to="/bikelist">Bike List</Link></li>
          </>
        )}
      </ul>
    </nav>
  );
};

export default Navbar;
