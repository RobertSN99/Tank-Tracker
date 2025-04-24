import { FaGithub, FaLinkedin } from "react-icons/fa";
import "./Footer.css";
import { FaRegFilePdf } from "react-icons/fa6";
import { useAuth } from "../../contexts/AuthContext";
import { useNavigate } from "react-router-dom";

function Footer() {
  const { user, logout } = useAuth();
  const isAdmin = user?.roles?.includes("Administrator");
  const navigate = useNavigate();
  return (
    <footer>
      <section className="left-section">
        <h3>Tank Tracker</h3>
        <p>A platform for tracking and managing tanks of World of Tanks.</p>
      </section>
      <section className="right-section">
        <ul>
          <span className="ul-title">Quick links</span>
          <li>
            <a href="/">Home</a>
          </li>
          {user ? (
            <>
              {isAdmin && (
                <li>
                  <a href="/admin">Admin</a>
                </li>
              )}
              <li>
                <a
                  onClick={async () => {
                    await logout();
                    navigate("/login");
                  }}
                >
                  Logout
                </a>
              </li>
            </>
          ) : (
            <>
              <li>
                <a href="login">Login</a>
              </li>
              <li>
                <a href="/register">Register</a>
              </li>
            </>
          )}
        </ul>
        <ul className="footer-contact">
          <span className="ul-title">Contact</span>
          <li>
            <FaRegFilePdf />
            <a href="" target="_blank">
              Resume
            </a>
          </li>
          <li>
            <FaLinkedin />
            <a
              href="https://www.linkedin.com/in/robert-spinoiu-06a9b120a/"
              target="_blank"
            >
              LinkedIn
            </a>
          </li>
          <li>
            <FaGithub />
            <a href="https://github.com/RobertSN99" target="_blank">
              GitHub
            </a>
          </li>
        </ul>
      </section>
      <section className="bottom-section">
        <p>Developed by © Robert-Nicușor Spînoiu</p>
      </section>
    </footer>
  );
}

export default Footer;
