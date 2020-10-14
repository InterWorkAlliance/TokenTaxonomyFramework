import React from 'react';
import {Layout, Menu} from 'antd';
import {Link} from "react-router-dom";
import options, {MenuOption, MenuSubOption} from './options';
import {SidebarWrapper} from './sidebar.style';

import {connect} from 'react-redux';

import { actionSelectEntity } from '../../actions';
import {ISidebarUI, IStoreState} from "../../store/IStoreState";

const {Sider} = Layout;
const SubMenu = Menu.SubMenu;



class Sidebar extends React.Component<ISidebarUI> {

  public getMenuItem = (singleOption: MenuOption) => {
    const {key, label, leftIcon, children} = singleOption;

    if (children) {
      return (
        <SubMenu
          key={key}
          title={
            <span className="menuHolder"  >
              <i className={leftIcon} />
              <span className="nav-text">
                {label}
              </span>
            </span>
          }
        >
          {
            children.map((child: MenuSubOption) => {
              const linkTo = `/${encodeURIComponent(child.key)}`;
              return (
                <Menu.Item key={child.key}>
                  <Link to={linkTo}>
                    {child.label}
                  </Link>
                </Menu.Item>
              );
            })}
        </SubMenu>
      );
    };

    return (
      <Menu.Item key={key}>
        <Link to={`/${key}`}>
          <span className="menuHolder" >
            <i className={leftIcon} />
            <span className="nav-text">
              {label}
            </span>
          </span>
        </Link>
      </Menu.Item>
    );
  };

  public generateMenuOption(menu: MenuOption) {
    if(this.props.state === 'LOADING') {
      return (<p>Loading...</p>);
    } else if (this.props.state === 'ERROR') {
      return (
        <Menu.Item key={Math.random()}>
          No Base
        </Menu.Item>
      );
    } else if (this.props.state === "LOADED") {
      if (menu.children) {
        return (
          <SubMenu
            key={menu.key}
            title={
              <span className="menuHolder"  >
                <i className={menu.leftIcon} />
                <span className="nav-text">
                  {menu.label}
                </span>
              </span>
            }
          >
            {
              menu.children.map((child: MenuSubOption) => {
                const linkTo = `/${child.key}`;
                return (
                  <Menu.Item key={child.key}>
                    <Link to={linkTo}>
                      {child.label}
                    </Link>
                  </Menu.Item>
                );
              })}
          </SubMenu>
        );
      }

      return (
        <Menu.Item key={menu.key}>
          <Link to={`/${menu.key}`}>
            <span className="menuHolder" >
              <i className={menu.leftIcon} />
              <span className="nav-text">
                {menu.label}
              </span>
            </span>
          </Link>
        </Menu.Item>
      );
    }
  };
  
  public generateMenuOptions = (key: string, label: string, prefix: string, bases: any[]) => {
    const baseOption: MenuOption = {
      key: key,
      label: label,
      leftIcon: 'ion-podium',
      children: []
    }
    if (bases === []) {
      return baseOption;
    } else {
      for (const base of bases) {
        const baseObj = base.toObject();
        if (baseObj.artifact !== undefined) {
          baseOption.children.push({
            key: prefix + "/" + baseObj.artifact.artifactSymbol.id,
            label: baseObj.artifact.name,
          });
        } 
      }
      return baseOption;
    }
  };

  public render() {
    const baseOptions = this.generateMenuOptions("bases", "Bases", "base", this.props.bases);
    const behaviorOptions = this.generateMenuOptions("behaviors", "Behaviors", "behavior", this.props.behaviors);
    const behaviorGroupOptions = this.generateMenuOptions("behaviorGroups", "Behavior Groups", "behaviorGroup", this.props.behaviorGroups);
    const propertySetOptions = this.generateMenuOptions("propertySets", "Property Sets", "propertySet", this.props.propertySets);
    const templateDefinitionOptions = this.generateMenuOptions("templateDefinitions", "Template Definitions", "templateDefinition", this.props.templateDefinitions);

    return (
      <SidebarWrapper className={'sidebar-custom'}>
        <a href="/"><h1 className="title">Token Taxonomy Editor</h1></a>
        <Sider 
          width={240} 
          className="sidebar"
          style={{ 
            background: '#fff',
            height: "calc(100vh - 70px)"
          }}
        >
          <Menu
            mode="inline"
            theme="light"
            defaultSelectedKeys={['base']}
            defaultOpenKeys={['base']}
            style={{borderRight: 0}}
            className="sidebarMenu"
          > 
            {this.generateMenuOption(baseOptions)}
            {this.generateMenuOption(behaviorOptions)}
            {this.generateMenuOption(behaviorGroupOptions)}
            {this.generateMenuOption(propertySetOptions)}
            {this.generateMenuOption(templateDefinitionOptions)}
            {options.map(singleOption =>
              this.getMenuItem(singleOption)
            )}

            <Menu.Item className="custom-menu-item">
              <Link to={`/create-artifact`}>
                <span className="menuHolder" >
                  <span className="nav-text">
                    Create Artifact
                  </span>
                </span>
              </Link>
            </Menu.Item>
          </Menu>
        </Sider>
      </SidebarWrapper>
    );
  }
}  

export default connect(
  (state: IStoreState) => {
    return {
      bases: state.ui.sidebarUI.bases,
      behaviors: state.ui.sidebarUI.behaviors,
      behaviorGroups: state.ui.sidebarUI.behaviorGroups,
      propertySets: state.ui.sidebarUI.propertySets,
      templateDefinitions: state.ui.sidebarUI.templateDefinitions,
      state: state.ui.sidebarUI.state,
      errorMsg: state.ui.sidebarUI.errorMsg,
    };
  },
  {
    selectEntity: actionSelectEntity
  })(Sidebar);
