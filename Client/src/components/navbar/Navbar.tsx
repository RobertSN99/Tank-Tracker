import { useState, useEffect } from "react";
import "./Navbar.css";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";

interface NavbarProps {
  isDark: boolean;
  setIsDark: (value: boolean) => void;
}

function Navbar({ isDark, setIsDark }: NavbarProps) {
  const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false);
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setIsDark(e.target.checked);
  };

  const handleHamburgerClick = () => {
    setIsMenuOpen((prev) => !prev);
  };

  const handleLogout = async () => {
    await logout();
    navigate("/login");
  };
  const isAdmin = user?.roles?.includes("Administrator");

  useEffect(() => {
    document.body.setAttribute("data-theme", isDark ? "dark" : "light");
  }, [isDark]);

  return (
    <nav className="navbar">
      <a href="/" className="navbar-logo">
        Tank Tracker
      </a>
      <ul className={`navbar-menu ${isMenuOpen ? "open" : ""}`}>
        <li className="navbar-item">
          <a href="/" className="navbar-link">
            Home
          </a>
        </li>

        {user ? (
          <>
            {isAdmin && (
              <li className="navbar-item">
                <a href="/admin" className="navbar-link">
                  Admin
                </a>
              </li>
            )}
            <li className="navbar-item user-info">
              <a
                className="navbar-user-name"
                onClick={() => navigate(`/user/${user.username}`)}
              >
                {user.username}
              </a>
            </li>
            <li className="navbar-item">
              <a
                className="navbar-link navbar-logout-btn"
                onClick={handleLogout}
              >
                Logout
              </a>
            </li>
          </>
        ) : (
          <>
            <li className="navbar-item">
              <a href="/login" className="navbar-link">
                Login
              </a>
            </li>
            <li className="navbar-item">
              <a href="/register" className="navbar-link">
                Register
              </a>
            </li>
          </>
        )}

        <li className="navbar-item navbar-theme-toggle">
          <label htmlFor="theme-check" className="theme-label">
            <input
              type="checkbox"
              id="theme-check"
              className="theme-toggle"
              onChange={handleChange}
              checked={isDark}
            />
            <span className="toggle-slider"></span>
          </label>
        </li>
      </ul>
      <div className="hamburger-menu" onClick={handleHamburgerClick}>
        <div></div>
        <div></div>
        <div></div>
      </div>
    </nav>
  );
}

export default Navbar;
