import { useNavigate } from "react-router-dom";
import "./Admin.css";

function Admin() {
  const navigate = useNavigate();
  return (
    <div className="admin-container">
      <ul className="admin-links-group">
        <li
          className="admin-link"
          onClick={() => {
            navigate("/create/tank");
          }}
        >
          <div className="admin-link-title">Tanks</div>
          <div className="admin-link-description">Add a new tank</div>
        </li>
        <li
          className="admin-link"
          onClick={() => {
            navigate("/create/nation");
          }}
        >
          <div className="admin-link-title">Nations</div>
          <div className="admin-link-description">Add a new nation</div>
        </li>
        <li
          className="admin-link"
          onClick={() => {
            navigate("/create/class");
          }}
        >
          <div className="admin-link-title">Classes</div>
          <div className="admin-link-description">Add a new class</div>
        </li>
        <li
          className="admin-link"
          onClick={() => {
            navigate("/create/tier");
          }}
        >
          <div className="admin-link-title">Tiers</div>
          <div className="admin-link-description">Add a new tier</div>
        </li>
        <li
          className="admin-link"
          onClick={() => {
            navigate("/create/status");
          }}
        >
          <div className="admin-link-title">Statuses</div>
          <div className="admin-link-description">Add a new status</div>
        </li>
      </ul>
    </div>
  );
}

export default Admin;
