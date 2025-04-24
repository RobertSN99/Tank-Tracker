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
import Register from "./pages/register/Register";
import Footer from "./components/footer/Footer";
import TankId from "./pages/tankId/TankId";
import Unauthorized from "./pages/unauthorized/Unauthorized";
import NotFound from "./pages/notfound/NotFound";
import Admin from "./pages/admin/Admin";
import ProtectedRoute from "./components/protectedRoute/ProtectedRoute";
import CreateTank from "./pages/create/tank/CreateTank";
import CreateNation from "./pages/create/nation/CreateNation";
import CreateClass from "./pages/create/class/CreateClass";
import CreateStatus from "./pages/create/status/CreateStatus";
import ScrollToTop from "./components/scrollToTop/ScrollToTop";
import UserProfile from "./pages/userProfile/UserProfile";

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
        {/* ScrollToTop component to reset scroll position on route change */}
        <ScrollToTop />
        <div className="App" data-theme={isDark ? "dark" : "light"}>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/home" element={<Home />} />
            <Route
              path="/admin"
              element={
                <ProtectedRoute requireRole="Administrator">
                  <Admin />
                </ProtectedRoute>
              }
            />
            <Route
              path="/create/tank"
              element={
                <ProtectedRoute requireRole="Administrator">
                  <CreateTank />
                </ProtectedRoute>
              }
            />
            <Route
              path="/create/nation"
              element={
                <ProtectedRoute requireRole="Administrator">
                  <CreateNation />
                </ProtectedRoute>
              }
            />
            <Route
              path="/create/class"
              element={
                <ProtectedRoute requireRole="Administrator">
                  <CreateClass />
                </ProtectedRoute>
              }
            />
            <Route
              path="/create/status"
              element={
                <ProtectedRoute requireRole="Administrator">
                  <CreateStatus />
                </ProtectedRoute>
              }
            />
            <Route
              path="/login"
              element={
                <ProtectedRoute guestOnly>
                  <Login />
                </ProtectedRoute>
              }
            />
            <Route
              path="/register"
              element={
                <ProtectedRoute guestOnly>
                  <Register />
                </ProtectedRoute>
              }
            />
            <Route
              path="/user/:userName"
              element={
                <ProtectedRoute allowedToUserOrAdmin>
                  <UserProfile />
                </ProtectedRoute>
              }
            />
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
