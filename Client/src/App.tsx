import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { useState, useEffect } from "react";
import "./App.css";
import Navbar from "./components/navbar/Navbar";
import Home from "./pages/home/Home";
import Login from "./pages/login/Login";
import Register from "./pages/logout/Register";
import Footer from "./components/footer/Footer";
import TankId from "./pages/tankId/TankId";
import Unauthorized from "./pages/unauthorized/Unauthorized";
import NotFound from "./pages/notfound/NotFound";
import Admin from "./pages/admin/Admin";

function App() {
  const [isDark, setIsDark] = useState<boolean>(
    localStorage.getItem("theme") === "dark"
  );

  // Sync the theme with the body and localStorage on theme change
  useEffect(() => {
    localStorage.setItem("theme", isDark ? "dark" : "light");
    document.body.setAttribute("data-theme", isDark ? "dark" : "light");
  }, [isDark]);

  return (
    <>
      <Router>
        <Navbar isDark={isDark} setIsDark={setIsDark} />
        <div className="App" data-theme={isDark ? "dark" : "light"}>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/home" element={<Home />} />
            <Route path="/admin" element={<Admin />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/tank/:id" element={<TankId />} />
            <Route path="/unauthorized" element={<Unauthorized />} />
            <Route path="/notfound" element={<NotFound />} />
            <Route path="*" element={<Navigate to={"/notfound"} />} />
          </Routes>
        </div>
        <Footer />
      </Router>
    </>
  );
}

export default App;
