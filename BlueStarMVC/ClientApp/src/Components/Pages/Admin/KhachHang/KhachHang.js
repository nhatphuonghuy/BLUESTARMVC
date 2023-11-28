import React, { useEffect, useState } from "react";
import './KhachHang.css';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { toast } from 'react-toastify';
import 'bootstrap/dist/css/bootstrap.min.css';

const KhachHang = () => {
    const [customers, setCustomers] = useState([]);
    const [selectedCustomers, setSelectedCustomers] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(8);
    const navigate = useNavigate();

    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentItems = customers.slice(indexOfFirstItem, indexOfLastItem);

    const paginate = pageNumber => setCurrentPage(pageNumber);

    const handleClick = () => {
        navigate('/KhachHang_Them'); 
    };

    useEffect(() => {
        fetch("api/customer/GetCustomers")
            .then(response => response.json())
            .then(responseJson => {
                console.log(responseJson);  
                setCustomers(responseJson);
            })
            .catch(error => {
                console.error("Error fetching data:", error);
            });
    }, [])

    


    useEffect(() => {
        // Lấy danh sách khách hàng từ API hoặc nguồn dữ liệu khác
        const fetchData = async () => {
            try {
                const response = await fetch("/api/customer/GetCustomers");
                const data = await response.json();
                setCustomers(data);
            } catch (error) {
                console.error("Error fetching data:", error);
            }
        };

        fetchData();
    }, []);

    
    const handleCheckboxChange = (customerId) => {
        if (selectedCustomers.includes(customerId)) {
            // Nếu đã chọn thì hủy chọn
            setSelectedCustomers([]);
        } else {
            // Nếu chưa chọn thì chọn
            setSelectedCustomers([customerId]);
        }
    };

  

    const handleShowInfo = async () => {
        try {
            if (selectedCustomers.length > 0) {
                const response = await fetch(`/api/customer/GetCustomerDetails?cIds=${selectedCustomers.join(',')}`);
                const data = await response.json();

                // Chuyển hướng sang trang sửa khách hàng và truyền thông tin khách hàng
                navigate('/KhachHang_Sua', { state: { selectedCustomerInfo: data } });
            } else {
                console.log("No customers selected.");
            }
        } catch (error) {
            console.error("Error fetching customer details:", error);
        }
    };

    const handleDelete = async () => {
        if (window.confirm("Are you sure to delete this customer")) {
            try {
                const response = await axios.delete('http://localhost:44430/api/customer', {
                    data: selectedCustomers, // Pass the array as data
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (response.status === 200) {
                    const updatedCustomers = customers.filter(customer => !selectedCustomers.includes(customer.cId));

                    // Cập nhật state để tái render bảng
                    setCustomers(updatedCustomers);

                    // Xóa danh sách khách hàng đã chọn
                    setSelectedCustomers([]);
                    toast.success('Customers deleted successfully');
                    
                } else {
                    toast.error('Failed to delete customers');
                }
            } catch (error) {
                toast.error('Error deleting customers: ' + error.message);
            }
        }
    };



    return (
        <div className="col-md-10 main">
            <div className="container mt-md-6">
                <div className="navbar d-flex justify-content-between align-items-center">
                    <h2 className="main-name mb-0">Khách hàng</h2>
                    {/* Actions: Đổi mật khẩu và Xem thêm thông tin */}
                    <div className="dropdown">
                        <a className="d-flex align-items-center dropdown-toggle" href="#" role="button" id="userDropdown" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i className="bi bi-person-circle" />
                        </a>
                        {/* Dropdown menu */}
                        <div className="dropdown-menu" aria-labelledby="userDropdown">
                            <a className="dropdown-item" href="#">Đổi mật khẩu</a>
                            <a className="dropdown-item" href="#">Xem thêm thông tin</a>
                        </div>
                    </div>
                </div>
    {/*thanh tìm kiếm với bộ lọc*/}
    <div className="find mt-5">
      <div className="d-flex w-100 justify-content-start align-items-center">
        <i className="bi bi-search" />
        <span className="first">
          <input className="form-control" placeholder="Tìm kiếm ..." />
        </span>
        <span className="second">Filters <i className="bi bi-chevron-compact-down" /></span>
      </div>
    </div>
    <table className="table table-bordered">
      <thead>
        <tr>
          <th />
          <th>C_ID</th>
          <th>Num_ID</th>
          <th>Tên khách hàng</th>
          <th>Điểm tích lũy</th>
          <th>Email</th>
        </tr>
      </thead>
                    <tbody>
                        {currentItems.map((item) => (
                            <tr key={item.cId}>
                                <td contentEditable="true" className="choose">
                                    <input
                                        className="form-check-input"
                                        type="checkbox"
                                        onChange={() => handleCheckboxChange(item.cId)}
                                        checked={selectedCustomers.includes(item.cId)}
                                    />
                                </td>
                                <td>{item.cId}</td>
                                <td>{item.numId}</td>
                                <td>{item.fullname}</td>
                                <td>{item.point}</td>
                                <td>{item.mail}</td>
                            </tr>
                        ))}
      </tbody>
    </table>
    {/*3 nut bam*/}
    <div className="d-flex justify-content-end my-3">
                    <button className="btn btn-primary mr-2" id="btnThem" onClick={handleClick}>Thêm</button>
                    <button className="btn btn-danger mr-2" id="btnXoa" onClick={handleDelete }>Xóa</button>
                    <button className="btn btn-warning" id="btnSua" onClick={handleShowInfo}>Sửa</button>
    </div>
                <ul className="pagination justify-content-center">
                    <li className="page-item ">
                        <a className="page-link" tabIndex={-1} onClick={() => paginate(currentPage - 1)}>Previous</a>
                    </li>
                    {[...Array(Math.ceil(customers.length / itemsPerPage)).keys()].map((number) => (
                        <li key={number} className={`page-item ${number + 1 === currentPage ? 'active' : ''}`}>
                            <a className="page-link"  onClick={() => paginate(number + 1)}>{number + 1}</a>
                        </li>
                    ))}
                    <li className="page-item">  
                        <a className="page-link" onClick={() => paginate(currentPage + 1)}>Next</a>
                    </li>
                </ul>
  </div>
</div>
    );
}
export default KhachHang;