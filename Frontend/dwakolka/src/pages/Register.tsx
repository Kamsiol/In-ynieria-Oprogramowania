import { useState } from "react";
import axios from "axios";

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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      
      await axios.post("http://localhost:5008/api/Account/register", formData, {
        headers: { "Content-Type": "application/json" }
      });
      
      setSuccess(true);
    } catch {
      setSuccess(false);
    }
  };

  return (
    <div className="max-w-lg mx-auto mt-10 p-6 border rounded-lg shadow-md bg-white">
      <h2 className="text-2xl font-bold text-center mb-6">Register</h2>
      <form onSubmit={handleSubmit}>
        <input name="email" type="email" placeholder="Email" value={formData.email} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required />
        <input name="password" type="password" placeholder="Password" value={formData.password} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required />
        <input name="name" type="text" placeholder="Name" value={formData.name} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required />
        <input name="surname" type="text" placeholder="Surname" value={formData.surname} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required />
        <input name="phoneNumber" type="tel" placeholder="Phone Number" value={formData.phoneNumber} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required />
        <input name="birthday" type="date" value={formData.birthday} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required />
        <select name="gender" value={formData.gender} onChange={handleChange} className="border p-2 w-full mb-3 rounded" required>
          <option value="">Select Gender</option>
          <option value="male">male</option>
          <option value="female">female</option>
        </select>
        <button type="submit" className="bg-blue-500 text-white py-2 px-4 w-full rounded hover:bg-blue-600 transition">Register</button>
      </form>

      {success === true && <p className="text-green-600 mt-3">Registration Successful!</p>}
      {success === false && <p className="text-red-600 mt-3">Registration Failed. Try Again.</p>}
    </div>
  );
};

export default Register;
