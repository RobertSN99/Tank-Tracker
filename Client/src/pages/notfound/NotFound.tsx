import "./NotFound.css";
import { useNavigate } from "react-router-dom";

function NotFound() {
  const navigate = useNavigate();
  return (
    <div className="notfound-container">
      <h1 className="notfound-title">Error 404 Not Found</h1>
      <p className="notfound-message">
        The page you are looking for could not be found :(
      </p>
      <button
        className="notfound-btn"
        onClick={() => {
          navigate("/");
        }}
      >
        Home
      </button>
    </div>
  );
}

export default NotFound;
