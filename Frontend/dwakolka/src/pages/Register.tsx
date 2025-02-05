import { useState } from "react";
import axios from "axios";
import "./Register.css"; // Подключаем CSS

const Register = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    name: "",
    surname: "",
    phoneNumber: "",
    birthday: "",
    gender: "",
  });
  const [success, setSuccess] = useState<boolean | null>(null);

  const API_URL = localStorage.getItem("API_URL");

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post(`${API_URL}/Account/register`, formData, {
        headers: { "Content-Type": "application/json" },
      });

      setSuccess(true);
    } catch {
      setSuccess(false);
    }
  };

  return (
    <div className="register-container">
      <h2 className="register-title">Register</h2>
      <form onSubmit={handleSubmit} className="register-form">
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
        <input
          name="name"
          type="text"
          placeholder="Name"
          value={formData.name}
          onChange={handleChange}
          className="input-field"
          required
        />
        <input
          name="surname"
          type="text"
          placeholder="Surname"
          value={formData.surname}
          onChange={handleChange}
          className="input-field"
          required
        />
        <input
          name="phoneNumber"
          type="tel"
          placeholder="Phone Number"
          value={formData.phoneNumber}
          onChange={handleChange}
          className="input-field"
          required
        />
        <input
          name="birthday"
          type="date"
          value={formData.birthday}
          onChange={handleChange}
          className="input-field"
          required
        />
        <select name="gender" value={formData.gender} onChange={handleChange} className="input-field" required>
          <option value="">Select Gender</option>
          <option value="male">Male</option>
          <option value="female">Female</option>
        </select>
        <button type="submit" className="register-button">Register</button>
      </form>

      {success === true && <p className="success-message">Registration Successful!</p>}
      {success === false && <p className="error-message">Registration Failed. Try Again.</p>}
    </div>
  );
};

export default Register;
