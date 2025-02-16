import { useState , useEffect} from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./Login.css"; // Подключаем CSS

const API_URL = localStorage.getItem("API_URL");

const Login = () => {
  const [userId, setUserId] = useState<string | null>(null);
  useEffect(() => {
   const storedUserId = localStorage.getItem("userId");
   if (storedUserId) {
     setUserId(storedUserId);
   }
 }, []);

  const [formData, setFormData] = useState({ email: "", password: "" });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post(`${API_URL}/Account/login`, formData, {
        headers: { "Content-Type": "application/json" },
      });

      // Получаем userId напрямую из API
      //const userId: string = response.data.userId;
      //localStorage.setItem("userId", userId);
      //костыль, но работает
      //Login request returns a string, instead of a bunch of parameters. need to parse it to use properly:
      var usrid:string  = response.data.toString().split(' ')[3];
      usrid = usrid.substring(0, usrid.length-1);
      localStorage.setItem("userId",usrid );
      setUserId(usrid);
      
      navigate("/"); // Перенаправление на главную
    } catch {
      setError("Invalid email or password");
    }
  };

  return (
    <div className="login-container">
      <h2 className="login-title">Login</h2>
      <form onSubmit={handleSubmit} className="login-form">
        <input
          name="email"
          type="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleChange}
          className="input-field"
          required
        />
        <input
          name="password"
          type="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleChange}
          className="input-field"
          required
        />
        <button type="submit" className="login-button">Login</button>
      </form>
      {error && <p className="error-message">{error}</p>}
    </div>
  );
};

export default Login;
