import React from "react";
import { Link, useNavigate } from "react-router-dom";

const Nav = (props: { name: string; setName: (name: string) => void }) => {
    const navigate = useNavigate();

    // Xử lý logout
    const logout = async () => {
        const response = await fetch('https://localhost:7242/api/Auth/logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
        });

        if (response.ok) {
            props.setName(""); // Đặt lại tên người dùng thành rỗng
            navigate("/login"); // Chuyển hướng người dùng đến trang login sau khi logout
        }
    };

    // Hiển thị menu dựa trên trạng thái đăng nhập của người dùng
    const menu = props.name === "" ? (
        <ul className="navbar-nav me-auto mb-2 mb-md-0">
            <li className="nav-item">
                <Link to="/login" className="nav-link">
                    Login
                </Link>
            </li>
            <li className="nav-item">
                <Link to="/register" className="nav-link">
                    Register
                </Link>
            </li>
        </ul>
    ) : (
        <ul className="navbar-nav me-auto mb-2 mb-md-0">
            <li className="nav-item">
                <Link to="/" className="nav-link" onClick={logout}>
                    Logout
                </Link>
            </li>
        </ul>
    );

    return (
        <nav className="navbar navbar-expand-md navbar-dark bg-dark mb-4">
            <div className="container-fluid">
                <Link to="/" className="navbar-brand">
                    Home
                </Link>
                <div>
                    {menu}
                </div>
            </div>
        </nav>
    );
};

export default Nav;
