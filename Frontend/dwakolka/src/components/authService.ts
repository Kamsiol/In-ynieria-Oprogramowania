import axios from "axios";

//const API_URL = "http://localhost:5008/api"; // Replace with your backend URL
const API_URL = localStorage.getItem("API_URL");

// User interface matching backend model
export interface User {
    name: string;
    surname: string;
    phoneNumber: number;
    email: string;
    mainCity: string;
    birthday: string; // Format: YYYY-MM-DD
  }

// Register a new user
export const registerUser = async (name: string) => {
    try {
      const response = await axios.post(`${API_URL}/Users`, { name });
  
      if (response.data.guid) {
        return response.data.guid; // Return the assigned GUID
      } else {
        throw new Error("Failed to register user");
      }
    } catch (error) {
      throw new Error("Error registering user");
    }
  };

// Authenticate with a GUID
export const authenticateWithGuid = async (guid: string) => {
  try {
    const response = await axios.post(`${API_URL}/Users`, { guid });

    if (response.data.token) {
      localStorage.setItem("token", response.data.token); // Store token
    }

    return response.data;
  } catch (error) {
    throw new Error("Invalid GUID");
  }
};

// Logout function
export const logout = () => {
  localStorage.removeItem("token");
};

// Check if user is authenticated
export const isAuthenticated = () => {
  return !!localStorage.getItem("token");
};
