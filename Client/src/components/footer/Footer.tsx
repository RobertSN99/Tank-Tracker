import { FaGithub, FaLinkedin } from "react-icons/fa";
import "./Footer.css";
import { FaRegFilePdf } from "react-icons/fa6";

function Footer() {
  const userIsLoggedIn: boolean = true;
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
          {userIsLoggedIn ? (
            <>
              <li>
                <a href="/admin">Admin</a>
              </li>
              <li>
                <a href="/logout">Logout</a>
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
