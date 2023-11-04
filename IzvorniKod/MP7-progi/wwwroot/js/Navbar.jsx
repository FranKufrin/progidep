function Navbar() {
    return (
        <div className='navbar-container'>
            <img alt="logo" src="../images/Dog_Paw_Print.png" className='logo-img' />
            <div className='navbar-inner-container'>
                <a className='navbar-anchor'>
                    <div className='left-container'>PO�ETNA</div>
                </a>
                <a className='navbar-anchor'>
                    <div className='middle-container'>SKLONI�TA</div>
                </a>
                <a className='navbar-anchor'>
                    <div className='right-container'>PRIJAVI SE</div>
                </a>
            </div>
        </div>
    )
}

export default Navbar