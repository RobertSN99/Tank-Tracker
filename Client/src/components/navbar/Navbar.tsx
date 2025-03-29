import { useState, useEffect } from "react";
import "./Navbar.css";

interface NavbarProps {
  isDark: boolean;
  setIsDark: (value: boolean) => void;
}

function Navbar({ isDark, setIsDark }: NavbarProps) {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const isChecked = event.target.checked;
    setIsDark(isChecked);
  };

  const handleHamburgerClick = () => {
    setIsMenuOpen((prev) => !prev);
  };

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
