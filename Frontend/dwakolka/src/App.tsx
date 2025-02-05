import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import Navbar from "./components/Navbar";
import AppRouter from "./components/AppRouter";

const App = () => {
  localStorage.setItem("API_URL", "http://localhost:5008/api");
  return (
    <Router>
      <div className="flex flex-col min-h-screen bg-gray-100">
        <Navbar />
        <main className="container mx-auto p-4 flex-grow">
          <AppRouter />
        </main>
      </div>
    </Router>
  );
};

export default App;
