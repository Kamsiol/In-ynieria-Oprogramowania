import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const [formData, setFormData] = useState({ email: "", password: "" });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    console.log("0");
    try {
      

      const response = await axios.post("http://localhost:5008/api/Account/login", formData, {
        headers: { "Content-Type": "application/json" }
      });
      const usrid:string  = response.data.toString().split(' ')[3];
      

      localStorage.setItem("userId", usrid.substring(0, usrid.length-1));
      navigate("/"); // Redirect to home
    } catch {
      setError("Invalid email or password");
    }
  };

  return (
    <div className="max-w-md mx-auto mt-10 p-6 border rounded-lg shadow-md bg-white">
      <h2 className="text-2xl font-bold text-center mb-6">Login</h2>
      <form onSubmit={handleSubmit}>
        <input name="email" type="email" placeholder="Email" value={formData.email} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required />
        <input name="password" type="password" placeholder="Password" value={formData.password} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required />
        <button type="submit" className="bg-blue-500 text-white py-2 px-4 w-full rounded hover:bg-blue-600 transition">Login</button>
      </form>
      {error && <p className="text-red-600 mt-3">{error}</p>}
    </div>
  );
};

export default Login;
