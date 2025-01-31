import React from 'react';
import './App.css';

import { BrowserRouter as Router } from "react-router-dom";
import Navbar from "./components/Navbar";
import AppRouter from "./components/AppRouter";


const App = () => {
  return (
    <Router>
      <Navbar />
      <div className="p-4">
        <AppRouter />
      </div>
    </Router>
  );
};

export default App;




/*
function App() {
  return (
    <div className="App">
      so wh8t n8w?
    </div>
  );
}

export default App;
*/